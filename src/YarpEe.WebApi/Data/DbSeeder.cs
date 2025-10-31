using YarpEe.Adapters.Persistence.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using HostEntity = YarpEe.Domain.Entities.Host;

namespace YarpEe.WebApi.Data;

public static class DbSeeder
{
    public static async Task SeedDefaultDataAsync(YarpEeDbContext context, ILogger logger)
    {
        // Check if default host exists
        var allHosts = await context.Hosts.ToListAsync();
        var defaultHost = allHosts.FirstOrDefault(h => h.Name.Value == "localhost");
        
        if (defaultHost == null)
        {
            logger.LogInformation("Creating default host 'localhost'...");
            
            var hostName = new YarpEe.Domain.ValueObjects.HostName("localhost");
            var host = new HostEntity(Guid.NewGuid(), hostName, "http://localhost", null);
            
            await context.Hosts.AddAsync(host);
            await context.SaveChangesAsync();
            
            logger.LogInformation("Default host created with ID: {HostId}", host.Id);
        }
    }
}
