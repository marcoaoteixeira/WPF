﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.DependencyInjection;
using Nameless.WPF.Entities;

namespace Nameless.WPF.Data;

/// <summary>
///     Auditing save changes interceptor.
/// </summary>
[ServiceLifetime(Lifetime = ServiceLifetime.Singleton)]
public sealed class UpdateAuditableEntitySaveChangesInterceptor : SaveChangesInterceptor {
    private readonly TimeProvider _timeProvider;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="UpdateAuditableEntitySaveChangesInterceptor"/> class.
    /// </summary>
    /// <param name="timeProvider">
    ///     The time provider.
    /// </param>
    public UpdateAuditableEntitySaveChangesInterceptor(TimeProvider timeProvider) {
        _timeProvider = Guard.Against.Null(timeProvider);
    }

    /// <inheritdoc />
    [SuppressMessage("ReSharper", "InvertIf", Justification = "Avoid duplicate the return statement.")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault", Justification = "We don't care for the other branches.")]
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default) {
        var dbContext = eventData.Context;
        if (dbContext is not null) {
            var entries = dbContext.ChangeTracker.Entries<Entity>();
            var now = _timeProvider.GetUtcNow();

            foreach (var entry in entries) {
                switch (entry.State) {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = now;
                        break;
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
