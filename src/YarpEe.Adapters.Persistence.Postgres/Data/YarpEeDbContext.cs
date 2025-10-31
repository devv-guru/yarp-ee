using Microsoft.EntityFrameworkCore;
using YarpEe.Domain.Entities;

namespace YarpEe.Adapters.Persistence.Postgres.Data;

public class YarpEeDbContext : DbContext
{
    public YarpEeDbContext(DbContextOptions<YarpEeDbContext> options) : base(options)
    {
    }

    public DbSet<Host> Hosts => Set<Host>();
    public DbSet<Cluster> Clusters => Set<Cluster>();
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<CertificateRef> Certificates => Set<CertificateRef>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YarpEeDbContext).Assembly);
    }
}
