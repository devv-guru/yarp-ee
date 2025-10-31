using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace YarpEe.Adapters.Persistence.Postgres.Data;

public class YarpEeDbContextFactory : IDesignTimeDbContextFactory<YarpEeDbContext>
{
    public YarpEeDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<YarpEeDbContext>();
        
        // Default connection string for design-time
        var connectionString = Environment.GetEnvironmentVariable("YARPEE__DB__CONNECTIONSTRING") 
            ?? "Host=localhost;Port=5432;Database=yarpee;Username=yarpee;Password=yarpee";
        
        optionsBuilder.UseNpgsql(connectionString);

        return new YarpEeDbContext(optionsBuilder.Options);
    }
}
