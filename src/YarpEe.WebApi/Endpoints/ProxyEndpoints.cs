using YarpEe.Application.Ports;

namespace YarpEe.WebApi.Endpoints;

public static class ProxyEndpoints
{
    public static void MapProxyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/proxy").WithTags("Proxy");

        group.MapPost("/reload", async (IProxyConfigReloader reloader) =>
        {
            await reloader.ReloadAsync();
            return Results.Ok(new { message = "Proxy configuration reloaded successfully" });
        });
    }
}
