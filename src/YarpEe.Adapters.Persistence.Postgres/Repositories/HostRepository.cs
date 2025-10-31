using Microsoft.EntityFrameworkCore;
using YarpEe.Application.Ports;
using YarpEe.Adapters.Persistence.Postgres.Data;
using YarpEe.Domain.Entities;

namespace YarpEe.Adapters.Persistence.Postgres.Repositories;

public class HostRepository : IHostRepository
{
    private readonly YarpEeDbContext _context;

    public HostRepository(YarpEeDbContext context)
    {
        _context = context;
    }

    public async Task<Host?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Hosts
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
    }

    public async Task<Host?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Hosts
            .FirstOrDefaultAsync(h => h.Name.Value == name, cancellationToken);
    }

    public async Task<IEnumerable<Host>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Hosts.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Host host, CancellationToken cancellationToken = default)
    {
        await _context.Hosts.AddAsync(host, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Host host, CancellationToken cancellationToken = default)
    {
        _context.Hosts.Update(host);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var host = await GetByIdAsync(id, cancellationToken);
        if (host != null)
        {
            _context.Hosts.Remove(host);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
