using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YarpEe.UI;
using YarpEe.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Port interfaces and Adapter implementations (Ports & Adapters architecture)
builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IClusterService, ClusterService>();
builder.Services.AddScoped<IHostService, HostService>();

await builder.Build().RunAsync();
