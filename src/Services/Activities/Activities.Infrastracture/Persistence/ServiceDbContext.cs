﻿using Activities.Domain.Aggregates.Activities;
using Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Activities.Infrastracture.Persistence;
internal sealed class ServiceDbContext : BaseDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<ActivityAggregate> Athletes => Set<ActivityAggregate>();

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
