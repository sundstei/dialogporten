﻿using Digdir.Library.Entity.EntityFrameworkCore.Features.Creatable;
using Digdir.Library.Entity.EntityFrameworkCore.Features.Identifiable;
using Digdir.Library.Entity.EntityFrameworkCore.Features.Lookup;
using Digdir.Library.Entity.EntityFrameworkCore.Features.SoftDeletable;
using Digdir.Library.Entity.EntityFrameworkCore.Features.Updatable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Digdir.Library.Entity.Abstractions.Features.Creatable;
using Digdir.Library.Entity.Abstractions.Features.EventPublisher;
using Digdir.Library.Entity.Abstractions.Features.Identifiable;
using Digdir.Library.Entity.Abstractions.Features.Lookup;
using Digdir.Library.Entity.Abstractions.Features.SoftDeletable;
using Digdir.Library.Entity.Abstractions.Features.Updatable;
using Digdir.Library.Entity.EntityFrameworkCore.Features.Immutable;
using Digdir.Library.Entity.EntityFrameworkCore.Features.Versionable;
using Digdir.Library.Entity.Abstractions.Features.Versionable;
using Digdir.Library.Entity.EntityFrameworkCore.Features.Aggregate;
using Digdir.Library.Entity.EntityFrameworkCore.Features.EventPublisher;

namespace Digdir.Library.Entity.EntityFrameworkCore;

/// <summary>
/// Provides extension methods for EntityFrameworkCore.
/// </summary>
public static class EntityLibraryEfCoreExtensions
{
    /// <summary>
    /// Updates the properties and sets the correct <see cref="EntityState"/> on the <see cref="ChangeTracker"/> for the entities implementing the following abstractions.
    /// <list type="bullet">
    ///     <item><see cref="ILookupEntity"/></item>
    ///     <item><see cref="IIdentifiableEntity"/></item>
    ///     <item><see cref="ICreatableEntity"/></item>
    ///     <item><see cref="IUpdateableEntity"/></item>
    ///     <item><see cref="ISoftDeletableEntity"/></item>
    ///     <item><see cref="IIdentifiableEntity"/></item>
    ///     <item><see cref="IVersionableEntity"/></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Should be called right before saving the entities.
    /// </remarks>
    /// <param name="changeTracker">The change tracker.</param>
    /// <param name="utcNow">The time in UTC in which the changes tok place.</param>
    /// <param name="cancellationToken">A token for requesting cancellation of the operation.</param>
    /// <returns>The same <see cref="ChangeTracker"/> instance so that multiple calls can be chained.</returns>
    public static async Task<ChangeTracker> HandleAuditableEntities(this ChangeTracker changeTracker, DateTimeOffset utcNow, CancellationToken cancellationToken = default)
    {
        changeTracker.HandleLookupEntities()
            .HandleIdentifiableEntities()
            .HandleImmutableEntities()
            //.HandleVersionableEntities()
            .HandleCreatableEntities(utcNow);
        //.HandleUpdatableEntities(utcNow);
        await changeTracker.HandleAggregateEntities(utcNow, cancellationToken);
        return changeTracker.HandleSoftDeletableEntities(utcNow);
    }

    /// <summary>
    /// Configures the shape of, and how the entities implementing the following abstractions are mapped to the database.
    /// <list type="bullet">
    ///     <item><see cref="ILookupEntity"/></item>
    ///     <item><see cref="IIdentifiableEntity"/></item>
    ///     <item><see cref="ICreatableEntity"/></item>
    ///     <item><see cref="IUpdateableEntity"/></item>
    ///     <item><see cref="ISoftDeletableEntity"/></item>
    ///     <item><see cref="IIdentifiableEntity"/></item>
    ///     <item><see cref="IVersionableEntity"/></item>
    ///     <item><see cref="IEventPublisher"/></item>
    /// </list>
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public static ModelBuilder AddAuditableEntities(this ModelBuilder modelBuilder)
    {
        return modelBuilder
            .EnableSoftDeletableQueryFilter()
            .AddAggregateEntities()
            .AddIdentifiableEntities()
            .AddImmutableEntities()
            .AddVersionableEntities()
            .AddUpdatableEntities()
            .AddCreatableEntities()
            .AddLookupEntities()
            .AddEventPublisher();
    }

    /// <summary>
    /// Removes pluralization in table names despite having pluralizing names on the contexts <see cref="DbSet{TEntity}"/> properties.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public static ModelBuilder RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(x => x.BaseType is null);

        foreach (var entity in entities)
        {
            entity.SetTableName(entity.DisplayName());
        }

        return modelBuilder;
    }

    /// <summary>
    /// Removes the maximum length restriction of data that is allowed in this property.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
    public static PropertyBuilder<string> HasUnlimitedLength(this PropertyBuilder<string> builder)
    {
        builder.Metadata.SetMaxLength(null);
        return builder;
    }

    internal static ModelBuilder EntitiesOfType<TEntity>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder> buildAction)
        where TEntity : class => modelBuilder.EntitiesOfType(typeof(TEntity), buildAction);

    internal static ModelBuilder EntitiesOfType(this ModelBuilder modelBuilder, Type type,
        Action<EntityTypeBuilder> buildAction)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(x => x.BaseType is null && type.IsAssignableFrom(x.ClrType));

        foreach (var entity in entities)
        {
            buildAction(modelBuilder.Entity(entity.ClrType));
        }

        return modelBuilder;
    }
}
