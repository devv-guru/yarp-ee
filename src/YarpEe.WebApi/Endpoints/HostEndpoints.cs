using YarpEe.Application.Ports;

namespace YarpEe.WebApi.Endpoints;

public static class HostEndpoints
{
    public static void MapHostEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/hosts").WithTags("Hosts");

        group.MapGet("/", async (IHostRepository hostRepository) =>
        {
            var hosts = await hostRepository.GetAllAsync();
            return Results.Ok(hosts.Select(h => new
            {
                id = h.Id,
                name = h.Name.Value,
                baseUrl = h.BaseUrl,
                certificateRef = h.CertificateRef,
                createdUtc = h.CreatedUtc,
                updatedUtc = h.UpdatedUtc
            }));
        });
    }
}
