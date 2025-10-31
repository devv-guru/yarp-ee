using YarpEe.Application.UseCases.Clusters;
using YarpEe.WebApi.DTOs;

namespace YarpEe.WebApi.Endpoints;

public static class ClusterEndpoints
{
    public static void MapClusterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clusters").WithTags("Clusters");

        group.MapGet("/", async (ListClusters listClusters) =>
        {
            var clusters = await listClusters.ExecuteAsync();
            return Results.Ok(clusters.Select(c => new ClusterResponse(
                c.Id,
                c.Name,
                c.LoadBalancingPolicy,
                c.CreatedUtc,
                c.UpdatedUtc,
                c.Destinations.Select(d => new DestinationResponse(d.Id, d.Address, d.HealthPath))
            )));
        });

        group.MapPost("/", async (CreateClusterRequest request, CreateCluster createCluster) =>
        {
            try
            {
                var destinations = request.Destinations?.Select(d =>
                    new CreateCluster.DestinationRequest(d.Address, d.HealthPath)
                );

                var cluster = await createCluster.ExecuteAsync(new CreateCluster.Request(
                    request.Name,
                    request.LoadBalancingPolicy,
                    destinations
                ));

                var response = new ClusterResponse(
                    cluster.Id,
                    cluster.Name,
                    cluster.LoadBalancingPolicy,
                    cluster.CreatedUtc,
                    cluster.UpdatedUtc,
                    cluster.Destinations.Select(d => new DestinationResponse(d.Id, d.Address, d.HealthPath))
                );

                return Results.Created($"/api/clusters/{cluster.Id}", response);
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

        group.MapPut("/{id:guid}", async (Guid id, UpdateClusterRequest request, UpdateCluster updateCluster) =>
        {
            try
            {
                var cluster = await updateCluster.ExecuteAsync(new UpdateCluster.Request(
                    id,
                    request.Name,
                    request.LoadBalancingPolicy
                ));

                var response = new ClusterResponse(
                    cluster.Id,
                    cluster.Name,
                    cluster.LoadBalancingPolicy,
                    cluster.CreatedUtc,
                    cluster.UpdatedUtc,
                    cluster.Destinations.Select(d => new DestinationResponse(d.Id, d.Address, d.HealthPath))
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

        group.MapDelete("/{id:guid}", async (Guid id, DeleteCluster deleteCluster) =>
        {
            try
            {
                await deleteCluster.ExecuteAsync(id);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
        });
    }
}
