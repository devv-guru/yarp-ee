using Microsoft.EntityFrameworkCore;
using YarpEe.Application.Ports;
using YarpEe.Adapters.Persistence.Postgres.Data;
using YarpEe.Domain.Entities;

namespace YarpEe.Adapters.Persistence.Postgres.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly YarpEeDbContext _context;

    public RouteRepository(YarpEeDbContext context)
    {
        _context = context;
    }

    public async Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Route>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Routes.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Route>> GetEnabledAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Routes
            .Where(r => r.Enabled)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Route route, CancellationToken cancellationToken = default)
    {
        await _context.Routes.AddAsync(route, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Route route, CancellationToken cancellationToken = default)
    {
        _context.Routes.Update(route);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var route = await GetByIdAsync(id, cancellationToken);
        if (route != null)
        {
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
