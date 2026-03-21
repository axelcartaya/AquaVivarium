using AquaVivarium.Components;
using AquaVivarium.Components.Account;
using AquaVivarium.Services;
using Data.Context;
using Data.Repositories;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

//Configuración de la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AquaVivariumContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//COnfiguración de Identity
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<AquaVivariumContext>() 
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

//SERVICIOS DE AUTENTICACIÓN Y BLAZOR
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

//MudBlazor
builder.Services.AddMudServices();

//Dependencias Repository
builder.Services.AddScoped<IPezRepository, PezRepository>();
builder.Services.AddScoped<IEspecieRepository, EspecieRepository>();

//Dependencias Services
builder.Services.AddScoped<IPezService, PezService>();

//Dependencias API
builder.Services.AddControllers();

//Aumentam el tamaño máximo de los mensajes que Blazor Server puede recibir para evitar problemas con la carga de imágenes o datos más grandes
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 1024 * 1024; // Lo subimos a 1MB
    });

//buscador de emails falso para que el registro funcione sin configurar un servidor SMTP
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

//JSON
// En el Program.cs del proyecto SERVER
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esta línea es la que rompe el bucle infinito
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        // Opcional: para que el JSON se vea bien en el navegador (opcional)
        options.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

//API
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AquaVivarium.Client._Imports).Assembly);

// Endpoints necesarios para que funcionen las páginas de Login/Register
app.MapAdditionalIdentityEndpoints();

app.Run();