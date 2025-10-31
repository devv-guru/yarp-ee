using YarpEe.Application.Ports;
using YarpEe.Domain.Entities;
using YarpEe.Domain.ValueObjects;

namespace YarpEe.Application.UseCases.Routes;

public class UpdateRoute
{
    private readonly IRouteRepository _routeRepository;
    private readonly IClusterRepository _clusterRepository;
    private readonly IProxyConfigReloader _proxyConfigReloader;

    public UpdateRoute(
        IRouteRepository routeRepository,
        IClusterRepository clusterRepository,
        IProxyConfigReloader proxyConfigReloader)
    {
        _routeRepository = routeRepository;
        _clusterRepository = clusterRepository;
        _proxyConfigReloader = proxyConfigReloader;
    }

    public async Task<Route> ExecuteAsync(Request request, CancellationToken cancellationToken = default)
    {
        var route = await _routeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (route == null)
            throw new InvalidOperationException($"Route with ID {request.Id} not found");

        if (request.ClusterId.HasValue && request.ClusterId.Value != route.ClusterId)
        {
            var cluster = await _clusterRepository.GetByIdAsync(request.ClusterId.Value, cancellationToken);
            if (cluster == null)
                throw new InvalidOperationException($"Cluster with ID {request.ClusterId} not found");

            route.UpdateCluster(request.ClusterId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Path))
        {
            var routePath = new RoutePath(request.Path);
            route.UpdatePath(routePath);
        }

        if (request.Order.HasValue)
        {
            route.UpdateOrder(request.Order.Value);
        }

        if (request.Methods != null)
        {
            route.SetMethods(request.Methods);
        }

        if (request.Enabled.HasValue)
        {
            if (request.Enabled.Value)
                route.Enable();
            else
                route.Disable();
        }

        await _routeRepository.UpdateAsync(route, cancellationToken);
        await _proxyConfigReloader.ReloadAsync(cancellationToken);

        return route;
    }

    public record Request(Guid Id, Guid? ClusterId = null, string? Path = null, int? Order = null, bool? Enabled = null, IEnumerable<string>? Methods = null);
}
