using YarpEe.Domain.Entities;

namespace YarpEe.Application.Ports;

public interface IClusterRepository
{
    Task<Cluster?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Cluster?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cluster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Cluster cluster, CancellationToken cancellationToken = default);
    Task UpdateAsync(Cluster cluster, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
