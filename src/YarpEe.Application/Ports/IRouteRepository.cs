using YarpEe.Domain.Entities;

namespace YarpEe.Application.Ports;

public interface IRouteRepository
{
    Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Route>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Route>> GetEnabledAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Route route, CancellationToken cancellationToken = default);
    Task UpdateAsync(Route route, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
