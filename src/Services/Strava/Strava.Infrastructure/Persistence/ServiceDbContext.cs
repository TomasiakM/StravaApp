using Common.Infrastructure.Persistence;
using Common.MessageBroker.Saga.DeleteActivity;
using Common.MessageBroker.Saga.ProcessActivityData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Strava.Infrastructure.Persistence;
internal sealed class ServiceDbContext : BaseDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<ProcessActivitySagaData> ProcessActivitySagaDatas => Set<ProcessActivitySagaData>();
    public DbSet<DeleteActivitySagaData> DeleteActivitySagaDatas => Set<DeleteActivitySagaData>();

    public ServiceDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DbConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProcessActivitySagaData>().HasKey(e => e.CorrelationId);
        modelBuilder.Entity<DeleteActivitySagaData>().HasKey(e => e.CorrelationId);
    }
}
