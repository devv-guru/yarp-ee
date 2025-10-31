using YarpEe.Application.UseCases.Routes;
using YarpEe.WebApi.DTOs;

namespace YarpEe.WebApi.Endpoints;

public static class RouteEndpoints
{
    public static void MapRouteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/routes").WithTags("Routes");

        group.MapGet("/", async (ListRoutes listRoutes) =>
        {
            var routes = await listRoutes.ExecuteAsync();
            return Results.Ok(routes.Select(r => new RouteResponse(
                r.Id,
                r.HostId,
                r.ClusterId,
                r.Path.Value,
                r.Order,
                r.Enabled,
                r.Methods
            )));
        });

        group.MapPost("/", async (CreateRouteRequest request, CreateRoute createRoute) =>
        {
            try
            {
                var route = await createRoute.ExecuteAsync(new CreateRoute.Request(
                    request.HostId,
                    request.ClusterId,
                    request.Path,
                    request.Order,
                    request.Enabled,
                    request.Methods
                ));

                var response = new RouteResponse(
                    route.Id,
                    route.HostId,
                    route.ClusterId,
                    route.Path.Value,
                    route.Order,
                    route.Enabled,
                    route.Methods
                );

                return Results.Created($"/api/routes/{route.Id}", response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateRouteRequest request, UpdateRoute updateRoute) =>
        {
            try
            {
                var route = await updateRoute.ExecuteAsync(new UpdateRoute.Request(
                    id,
                    request.ClusterId,
                    request.Path,
                    request.Order,
                    request.Enabled,
                    request.Methods
                ));

                var response = new RouteResponse(
                    route.Id,
                    route.HostId,
                    route.ClusterId,
                    route.Path.Value,
                    route.Order,
                    route.Enabled,
                    route.Methods
                );

                return Results.Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapDelete("/{id:guid}", async (Guid id, DeleteRoute deleteRoute) =>
        {
            try
            {
                await deleteRoute.ExecuteAsync(id);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
        });
    }
}
