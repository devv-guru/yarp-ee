using Microsoft.EntityFrameworkCore;
using YarpEe.Application.Ports;
using YarpEe.Adapters.Persistence.Postgres.Data;
using YarpEe.Domain.Entities;

namespace YarpEe.Adapters.Persistence.Postgres.Repositories;

public class ClusterRepository : IClusterRepository
{
    private readonly YarpEeDbContext _context;

    public ClusterRepository(YarpEeDbContext context)
    {
        _context = context;
    }

    public async Task<Cluster?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clusters
            .Include(c => c.Destinations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cluster?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Clusters
            .Include(c => c.Destinations)
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Cluster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clusters
            .Include(c => c.Destinations)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Cluster cluster, CancellationToken cancellationToken = default)
    {
        await _context.Clusters.AddAsync(cluster, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Cluster cluster, CancellationToken cancellationToken = default)
    {
        _context.Clusters.Update(cluster);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cluster = await GetByIdAsync(id, cancellationToken);
        if (cluster != null)
        {
            _context.Clusters.Remove(cluster);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
