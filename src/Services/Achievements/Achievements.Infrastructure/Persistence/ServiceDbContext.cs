using Achievements.Domain.Aggregates.Achievement;
using Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Achievements.Infrastructure.Persistence;
internal sealed class ServiceDbContext : BaseDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Achievement> Achievements => Set<Achievement>();

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
