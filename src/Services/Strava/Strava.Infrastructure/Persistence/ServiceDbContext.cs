using Common.Infrastructure.Persistence;
using Common.MessageBroker.Saga.ProcessActivityData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Strava.Infrastructure.Persistence;
internal sealed class ServiceDbContext : BaseDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<ProcessActivitySagaData> SagaDatas => Set<ProcessActivitySagaData>();

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
    }
}
