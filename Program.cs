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

builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<ApiClient>();
builder.Services.AddScoped<AuthenticationStateProvider, ReaAuthStateProvider>();

builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<ClienteService>();
builder.Services.AddSingleton<ChecklistService>();
builder.Services.AddSingleton<DashboardService>();
builder.Services.AddSingleton<XmlService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://rea-interno-production.up.railway.app") });

await builder.Build().RunAsync();
