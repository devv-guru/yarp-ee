using YarpEe.Domain.Entities;

namespace YarpEe.Application.Ports;

public interface IHostRepository
{
    Task<Host?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Host?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Host>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Host host, CancellationToken cancellationToken = default);
    Task UpdateAsync(Host host, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
