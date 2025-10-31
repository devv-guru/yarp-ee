using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace YarpEe.WebApi.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => false
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = _ => true
        });
    }
}
