﻿using Core.Application.ServiceLifeTimes;
using Core.Domain.BaseEvents;
using Core.Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Core.Infrastructure.Context;

public abstract class CleanBaseDbContext<TContext> : DbContext, ICleanBaseIgnore
    where TContext : DbContext
{
    private readonly IPublisher _publisher;
    protected CleanBaseDbContext(DbContextOptions<TContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Auditing();
        await PublishDomainEventAsync();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<ICleanBaseDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(TContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    #region Private
    private void Auditing()
    {
        var entries = ChangeTracker.Entries<ICleanAuditable>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(e => e.CreatedAt).CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(e => e.ModifiedAt).CurrentValue = DateTime.Now;
            }
        }
    }
    private async Task PublishDomainEventAsync()
    {
        var entitiesWithDomainEvents = ChangeTracker.Entries<ICleanBaseHasDomainEvent>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entitiesWithDomainEvents
            .SelectMany(entity => entity.DomainEvents)
            .ToList();

        entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
    #endregion
}
