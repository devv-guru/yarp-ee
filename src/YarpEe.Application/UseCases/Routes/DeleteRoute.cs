using YarpEe.Application.Ports;

namespace YarpEe.Application.UseCases.Routes;

public class DeleteRoute
{
    private readonly IRouteRepository _routeRepository;
    private readonly IProxyConfigReloader _proxyConfigReloader;

    public DeleteRoute(IRouteRepository routeRepository, IProxyConfigReloader proxyConfigReloader)
    {
        _routeRepository = routeRepository;
        _proxyConfigReloader = proxyConfigReloader;
    }

    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var route = await _routeRepository.GetByIdAsync(id, cancellationToken);
        if (route == null)
            throw new InvalidOperationException($"Route with ID {id} not found");

        await _routeRepository.DeleteAsync(id, cancellationToken);
        await _proxyConfigReloader.ReloadAsync(cancellationToken);
    }
}
