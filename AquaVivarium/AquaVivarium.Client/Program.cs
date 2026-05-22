using AquaVivarium.Client.Services;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();
builder.Services.AddMudServices();

// Registro del HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

//Dependicias Services
builder.Services.AddScoped<IPezService, PezServiceClient>();
builder.Services.AddScoped<IEspecieService, EspecieServiceClient>();
builder.Services.AddScoped<ICategoriaGuiaService, CategoriaGuiaServiceClient>();
builder.Services.AddScoped<IPlantaService, PlantaServiceClient>();
builder.Services.AddScoped<IGuiaService, GuiaServiceClient>();
builder.Services.AddScoped<IAcuarioService, AcuarioServiceClient>();
builder.Services.AddScoped<IAcuarioIAService, AcuarioIAServiceClient>();

await builder.Build().RunAsync();


