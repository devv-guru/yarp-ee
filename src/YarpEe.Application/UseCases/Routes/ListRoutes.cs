using YarpEe.Application.Ports;
using YarpEe.Domain.Entities;

namespace YarpEe.Application.UseCases.Routes;

public class ListRoutes
{
    private readonly IRouteRepository _routeRepository;

    public ListRoutes(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<IEnumerable<Route>> ExecuteAsync(bool enabledOnly = false, CancellationToken cancellationToken = default)
    {
        return enabledOnly
            ? await _routeRepository.GetEnabledAsync(cancellationToken)
            : await _routeRepository.GetAllAsync(cancellationToken);
    }
}
