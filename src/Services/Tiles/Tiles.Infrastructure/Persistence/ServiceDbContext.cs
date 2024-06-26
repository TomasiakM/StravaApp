﻿using Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Infrastructure.Persistence;
internal sealed class ServiceDbContext : BaseDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<ActivityTilesAggregate> Tiles => Set<ActivityTilesAggregate>();
    public DbSet<CoordinatesAggregate> Coordinates => Set<CoordinatesAggregate>();

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
