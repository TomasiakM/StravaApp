using Athletes.Domain.Aggregates.Athletes;
using Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Athletes.Infrastructure.Persistence;
internal class ServiceDbContext : BaseDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<AthleteAggregate> Athletes => Set<AthleteAggregate>();

    public ServiceDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("DbConnection"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
