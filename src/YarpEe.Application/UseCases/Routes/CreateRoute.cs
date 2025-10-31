using YarpEe.Application.Ports;
using YarpEe.Domain.Entities;
using YarpEe.Domain.ValueObjects;

namespace YarpEe.Application.UseCases.Routes;

public class CreateRoute
{
    private readonly IRouteRepository _routeRepository;
    private readonly IClusterRepository _clusterRepository;
    private readonly IHostRepository _hostRepository;
    private readonly IProxyConfigReloader _proxyConfigReloader;

    public CreateRoute(
        IRouteRepository routeRepository,
        IClusterRepository clusterRepository,
        IHostRepository hostRepository,
        IProxyConfigReloader proxyConfigReloader)
    {
        _routeRepository = routeRepository;
        _clusterRepository = clusterRepository;
        _hostRepository = hostRepository;
        _proxyConfigReloader = proxyConfigReloader;
    }

    public async Task<Route> ExecuteAsync(Request request, CancellationToken cancellationToken = default)
    {
        var cluster = await _clusterRepository.GetByIdAsync(request.ClusterId, cancellationToken);
        if (cluster == null)
            throw new InvalidOperationException($"Cluster with ID {request.ClusterId} not found");

        var host = await _hostRepository.GetByIdAsync(request.HostId, cancellationToken);
        if (host == null)
            throw new InvalidOperationException($"Host with ID {request.HostId} not found");

        var routePath = new RoutePath(request.Path);
        var route = new Route(Guid.NewGuid(), request.HostId, request.ClusterId, routePath, request.Order, request.Enabled);

        if (request.Methods != null && request.Methods.Any())
        {
            route.SetMethods(request.Methods);
        }

        await _routeRepository.AddAsync(route, cancellationToken);
        await _proxyConfigReloader.ReloadAsync(cancellationToken);

        return route;
    }

    public record Request(Guid HostId, Guid ClusterId, string Path, int Order = 0, bool Enabled = true, IEnumerable<string>? Methods = null);
}
