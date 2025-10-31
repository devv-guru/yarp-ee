using Microsoft.EntityFrameworkCore;
using YarpEe.Adapters.Persistence.Postgres.Data;
using YarpEe.Adapters.Persistence.Postgres.Repositories;
using YarpEe.Adapters.Proxy.Yarp.Configuration;
using YarpEe.Application.Ports;
using YarpEe.Application.UseCases.Routes;
using YarpEe.Application.UseCases.Clusters;
using YarpEe.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure PostgreSQL
var connectionString = builder.Configuration["YARPEE__DB__CONNECTIONSTRING"] 
    ?? "Host=localhost;Port=5432;Database=yarpee;Username=yarpee;Password=yarpee";

builder.Services.AddDbContext<YarpEeDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories (adapters)
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IClusterRepository, ClusterRepository>();
builder.Services.AddScoped<IHostRepository, HostRepository>();

// Register YARP config provider
builder.Services.AddSingleton<DynamicProxyConfigProvider>();
builder.Services.AddSingleton<IProxyConfigReloader, YarpProxyConfigReloader>();

// Register use cases
builder.Services.AddScoped<CreateRoute>();
builder.Services.AddScoped<UpdateRoute>();
builder.Services.AddScoped<DeleteRoute>();
builder.Services.AddScoped<ListRoutes>();
builder.Services.AddScoped<CreateCluster>();
builder.Services.AddScoped<UpdateCluster>();
builder.Services.AddScoped<DeleteCluster>();
builder.Services.AddScoped<ListClusters>();

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromMemory(Array.Empty<Yarp.ReverseProxy.Configuration.RouteConfig>(), Array.Empty<Yarp.ReverseProxy.Configuration.ClusterConfig>());

builder.Services.AddSingleton<Yarp.ReverseProxy.Configuration.IProxyConfigProvider>(sp =>
    sp.GetRequiredService<DynamicProxyConfigProvider>());

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<YarpEeDbContext>();

var app = builder.Build();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<YarpEeDbContext>();
    try
    {
        await dbContext.Database.MigrateAsync();
        app.Logger.LogInformation("Database migration completed successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Failed to migrate database");
        throw;
    }
}

// Configure middleware
app.UseRouting();

// Map endpoints
app.MapRouteEndpoints();
app.MapClusterEndpoints();
app.MapProxyEndpoints();
app.MapHealthEndpoints();

// Map YARP reverse proxy
app.MapReverseProxy();

app.Run();
