using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using REA.Web;
using REA.Web.Providers;
using REA.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<AuthenticationStateProvider, ReaAuthStateProvider>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ChecklistService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<XmlService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://rea-interno-production.up.railway.app") });

await builder.Build().RunAsync();
