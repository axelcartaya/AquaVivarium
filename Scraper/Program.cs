using System.Text.RegularExpressions;
using Microsoft.Playwright;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Data.Context;
using Domain.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using GTranslate.Translators;

// Configuracuón de rutas y bse de datos
var builder = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = builder.Build();
var connectionString = configuration.GetConnectionString("DefaultConnection");

string executionPath = AppDomain.CurrentDomain.BaseDirectory;
string rootPath = Path.GetFullPath(Path.Combine(executionPath, "..", "..", ".."));

// ruta relativa
string baseImagePath = Path.Combine(rootPath, "AquaVivarium", "AquaVivarium", "wwwroot", "imagenes", "imagenesEspecie");

var optionsBuilder = new DbContextOptionsBuilder<AquaVivariumContext>();
optionsBuilder.UseSqlServer(connectionString);

using var context = new AquaVivariumContext(optionsBuilder.Options);
using var httpClient = new HttpClient();

httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
httpClient.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9,en;q=0.8");

Console.WriteLine("=== AQUAVIVARIUM FULL SCRAPER (AQUASABI INGLÉS -> ESPAÑOL) ===");

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
var browserContext = await browser.NewContextAsync(new BrowserNewContextOptions { Locale = "en-US" });
var page = await browserContext.NewPageAsync();

var listaUrls = await ObtenerUrlsCatalogoAquasabi(page);

Console.WriteLine($"\n✅ {listaUrls.Count} plantas encontradas en Aquasabi. ENTER para empezar a importar.");
Console.ReadLine();

var nombresProcesados = new HashSet<string>();
int procesadas = 0;

foreach (var urlPlanta in listaUrls)
{
    procesadas++;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n[{procesadas}/{listaUrls.Count}] {urlPlanta}");
    Console.ResetColor();

    await ImportarPlantaAquasabi(page, urlPlanta, nombresProcesados);
    await Task.Delay(1500);
}

Console.WriteLine("\n✅ PROCESO COMPLETADO.");
Console.ReadLine();


// métodos del scraper

async Task<List<string>> ObtenerUrlsCatalogoAquasabi(IPage page)
{
    var urls = new HashSet<string>();
    int paginaActual = 1;
    bool hayMasPaginas = true;

    Console.WriteLine("Navegando al catálogo de Aquasabi...");

    // lista negra
    var palabrasProhibidas = new[] {
        "aquatic-plants-", "aquarium-", "glassware", "co2", "filter", "tools",
        "nutrition", "layout", "sale", "new", "blog", "brands", "accessories",
        "home", "cart", "register", "login", "account", "contact", "faq",
        "shipping", "imprint", "privacy", "terms", "withdrawal", "checkout", "forgot"
    };

    while (hayMasPaginas && paginaActual <= 30)
    {
        Console.WriteLine($"Explorando página {paginaActual}...");
        string urlPagina = $"https://www.aquasabi.com/aquatic-plants?page={paginaActual}";

        await page.GotoAsync(urlPagina, new PageGotoOptions { WaitUntil = WaitUntilState.Load, Timeout = 60000 });
        await Task.Delay(2000);

        var jsCode = @"
            () => {
                let links = [];
                // Aquasabi envuelve los productos en divs con clases como 'item-box' o usa 'a' con clase 'img-block'
                document.querySelectorAll('.item-box a, .product a, .caption a').forEach(a => {
                    let href = a.href;
                    if (href && href.startsWith('https://www.aquasabi.com/') && href.split('/').length === 4) {
                        if (!href.includes('?')) {
                            links.push(href);
                        }
                    }
                });

                // Si por algún motivo cambiaron las clases, usamos el plan B genérico evitando menús
                if(links.length === 0){
                    document.querySelectorAll('a').forEach(a => {
                        if (a.closest('header') || a.closest('footer') || a.closest('nav') || a.closest('.sidebar')) return;
                        let href = a.href;
                        if (href && href.startsWith('https://www.aquasabi.com/') && href.split('/').length === 4 && !href.includes('?')) {
                            links.push(href);
                        }
                    });
                }

                return Array.from(new Set(links));
            }
        ";

        var urlsNuevas = await page.EvaluateAsync<string[]>(jsCode);
        int contadorAntes = urls.Count;

        if (urlsNuevas.Length > 0)
        {
            foreach (var url in urlsNuevas)
            {
                string urlLower = url.ToLower();

                bool esBasura = palabrasProhibidas.Any(palabra => urlLower.Contains(palabra)) || urlLower == "https://www.aquasabi.com/aquatic-plants";

                if (!esBasura)
                {
                    urls.Add(url);
                }
            }
            if (urls.Count == contadorAntes)
            {
                Console.WriteLine("   [!] Fin del catálogo detectado (No hay plantas nuevas).");
                hayMasPaginas = false;
            }
            else
            {
                paginaActual++;
            }
        }
        else
        {
            Console.WriteLine("   [!] Página vacía detectada.");
            hayMasPaginas = false;
        }
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n✅ Escaneo completo: Se han encontrado {urls.Count} plantas únicas en Aquasabi.");
    Console.ResetColor();

    return urls.ToList();
}

async Task ImportarPlantaAquasabi(IPage page, string url, HashSet<string> procesados)
{
    try
    {
        await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.Load, Timeout = 60000 });
        await Task.Delay(1500);

        var htmlReal = await page.ContentAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlReal);

        var nombreCrudo = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText.Trim();
        if (string.IsNullOrEmpty(nombreCrudo)) return;

        string nombreCientifico = GetAquasabiValue(doc, "botanical name");
        if (nombreCientifico == "Not specified") nombreCientifico = nombreCrudo.Split('-')[0].Trim();

        if (procesados.Contains(nombreCientifico) || await context.Especies.AnyAsync(e => e.NombreCientifico == nombreCientifico))
        {
            Console.WriteLine($"   [OMITIDA] {nombreCientifico} ya procesada.");
            return;
        }

        // Extracción atributos
        string rawLuz = GetAquasabiValue(doc, "Light");
        string rawCo2 = GetAquasabiValue(doc, "CO2"); 
        string rawDif = GetAquasabiValue(doc, "Difficulty");
        string rawCrec = GetAquasabiValue(doc, "Growth");
        string rawAlt = GetAquasabiValue(doc, "Height");

        string luzEs = rawLuz.ToLower().Contains("high") || rawLuz.ToLower().Contains("intensive") ? "Alta" :
                       rawLuz.ToLower().Contains("medium") ? "Media" :
                       rawLuz.ToLower().Contains("low") ? "Baja" : "No especificada";

        bool necesitaCo2 = rawCo2.ToLower().Contains("yes");

        string dificultadEs = rawDif.ToLower().Contains("easy") ? "Fácil" :
                              rawDif.ToLower().Contains("medium") ? "Media" :
                              rawDif.ToLower().Contains("hard") || rawDif.ToLower().Contains("advanced") ? "Difícil" : "No especificada";

        string crecimientoEs = rawCrec.ToLower().Contains("fast") ? "Rápido" :
                               rawCrec.ToLower().Contains("medium") ? "Medio" :
                               rawCrec.ToLower().Contains("slow") ? "Lento" : "No especificado";

        var nodosDesc = doc.DocumentNode.SelectNodes("//div[contains(@class, 'description')]//p")
                        ?? doc.DocumentNode.SelectNodes("//div[@id='description']//p");
        string descOriginal = nodosDesc != null ? string.Join("\n\n", nodosDesc.Select(n => n.InnerText.Trim())) : "";
        string descEs = descOriginal;

        if (!string.IsNullOrEmpty(descOriginal))
        {
            try
            {
                var translator = new GoogleTranslator();
                var result = await translator.TranslateAsync(descOriginal, "es", "en");
                descEs = result.Translation;
            }
            catch { }
        }

        // creación especie
        var especie = new Especie
        {
            Nombre = nombreCrudo.Split('-')[0].Trim(),
            NombreCientifico = nombreCientifico,
            Descripcion = descEs,
            TipoEspecie = "Planta",
            Familia = GetAquasabiValue(doc, "Family"),
            Genero = GetAquasabiValue(doc, "Genus"),
            Dificultad = dificultadEs,
            TempMin = 18,
            TempMax = 28,
            PhMin = 6.0m,
            PhMax = 7.5m,
            GhMin = 2,
            GhMax = 15,
            Planta = new Planta
            {
                Crecimiento = crecimientoEs,
                Iluminacion = luzEs,
                NecesitaCo2 = necesitaCo2,
                Altura = (!rawAlt.Contains("Yes") && !rawAlt.Contains("No")) ? rawAlt : "No especificada"
            }
        };

        context.Especies.Add(especie);
        await context.SaveChangesAsync();

        procesados.Add(nombreCientifico);

        await ProcesarImagenesAquasabi(page, especie.Id, especie.Nombre);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"   [OK] {especie.Nombre} | Luz: {luzEs} | CO2: {(necesitaCo2 ? "Sí" : "No")} | Crec: {crecimientoEs}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"   [ERROR] {url}: {ex.Message}");
    }
}

// Helpers
string GetAquasabiValue(HtmlDocument doc, string label)
{
    var lowerLabel = label.ToLower();

    var spanNode = doc.DocumentNode.SelectSingleNode($"//span[contains(@class, 'item-variation-name') and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{lowerLabel}')]/following-sibling::span");

    if (spanNode != null) return LimpiarTexto(spanNode.InnerText);

    var techNode = doc.DocumentNode.SelectSingleNode($"//*[(self::dt or self::td or self::div) and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{lowerLabel}')]/following-sibling::*[1]");

    if (techNode != null) return LimpiarTexto(techNode.InnerText);

    return "Not specified";
}

string LimpiarTexto(string t) =>
    string.IsNullOrEmpty(t) ? "Not specified" : System.Net.WebUtility.HtmlDecode(Regex.Replace(t, @"\s+", " ").Trim());

(int? Min, int? Max) ParseRange(string input)
{
    if (string.IsNullOrEmpty(input) || input == "Not specified") return (null, null);
    var matches = Regex.Matches(input, @"\d+");
    if (matches.Count >= 2) return (int.Parse(matches[0].Value), int.Parse(matches[1].Value));
    if (matches.Count == 1) return (int.Parse(matches[0].Value), int.Parse(matches[0].Value));
    return (null, null);
}

(decimal? Min, decimal? Max) ParseRangeDecimal(string input)
{
    if (string.IsNullOrEmpty(input) || input == "Not specified") return (null, null);
    var textLimpio = input.Replace(",", ".");
    var matches = Regex.Matches(textLimpio, @"[0-9]+(\.[0-9]+)?");
    try
    {
        if (matches.Count >= 2) return (decimal.Parse(matches[0].Value, System.Globalization.CultureInfo.InvariantCulture), decimal.Parse(matches[1].Value, System.Globalization.CultureInfo.InvariantCulture));
        if (matches.Count == 1) return (decimal.Parse(matches[0].Value, System.Globalization.CultureInfo.InvariantCulture), decimal.Parse(matches[0].Value, System.Globalization.CultureInfo.InvariantCulture));
    }
    catch { return (null, null); }
    return (null, null);
}

string? ObtenerFamiliaPorGenero(string genero)
{
    genero = genero.Trim();
    return genero switch
    {
        "Anubias" or "Bucephalandra" or "Cryptocoryne" => "Araceae",
        "Echinodorus" or "Sagittaria" => "Alismataceae",
        "Hygrophila" or "Staurogyne" => "Acanthaceae",
        "Microsorum" or "Bolbitis" => "Polypodiaceae",
        "Vallisneria" or "Egeria" => "Hydrocharitaceae",
        "Ludwigia" => "Onagraceae",
        "Rotala" or "Ammania" => "Lythraceae",
        "Alternanthera" => "Amaranthaceae",
        "Micranthemum" => "Linderniaceae",
        "Taxiphyllum" or "Vesicularia" => "Hypnaceae",
        _ => null
    };
}
async Task ProcesarImagenesAquasabi(IPage page, int especieId, string nombreCientifico)
{
    string folderPath = Path.Combine(baseImagePath, especieId.ToString());
    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

    int contador = 0;
    var urlsUnicas = new HashSet<string>();

    await page.EvaluateAsync("window.scrollTo(0, 500)");
    await Task.Delay(500);

    var jsCode = @"
        () => {
            let urls = [];
            document.querySelectorAll('img').forEach(img => {
                // Aquasabi usa atributos normales, pero pillamos todo por si acaso
                let src = img.getAttribute('src');
                let dataSrc = img.getAttribute('data-src');
                let og = document.querySelector('meta[property=""og:image""]');
                
                if (og && og.content) urls.push(og.content); // La mejor calidad siempre va primero
                if (dataSrc) urls.push(dataSrc);
                if (src) urls.push(src);
            });
            return Array.from(new Set(urls));
        }
    ";

    var urlsExtraidas = await page.EvaluateAsync<string[]>(jsCode);

    foreach (var imgUrl in urlsExtraidas)
    {
        string urlLower = imgUrl.ToLower();

        if (urlsUnicas.Contains(imgUrl)) continue;

        if (urlLower.Contains("logo") || urlLower.Contains("icon") || urlLower.Contains("svg") ||
            urlLower.Contains("avatar") || urlLower.Contains("banner") || urlLower.Contains("data:image"))
        {
            continue;
        }

        try
        {
            urlsUnicas.Add(imgUrl);
            string fullUrl = imgUrl.StartsWith("http") ? imgUrl : $"https://www.aquasabi.com{imgUrl}";

            var cleanUrl = fullUrl.Split('?')[0];

            byte[] bytes = await httpClient.GetByteArrayAsync(cleanUrl);
            using var image = Image.Load(bytes);

            if (image.Width < 200 || image.Height < 200) continue;

            image.Mutate(x => x.Resize(new ResizeOptions { Size = new Size(1200, 0), Mode = ResizeMode.Max }));

            string fileName = contador == 0 ? "principal.webp" : $"foto_{contador}.webp";
            string pathFisico = Path.Combine(folderPath, fileName);

            await image.SaveAsWebpAsync(pathFisico);

            context.EspecieImagenes.Add(new EspecieImagen
            {
                EspecieId = especieId,
                Url = $"/imagenes/imagenesEspecie/{especieId}/{fileName}",
                AltText = $"Planta {nombreCientifico}"
            });

            contador++;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"      [IMAGEN] ✓ Foto {contador} guardada en alta resolución.");
            Console.ResetColor();

            if (contador >= 3) break;
        }
        catch
        {
            continue;
        }
    }

    if (contador > 0)
    {
        await context.SaveChangesAsync();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("      [!] No se encontraron fotos válidas de la planta.");
        Console.ResetColor();
    }
}