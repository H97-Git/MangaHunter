using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Domain.Primitives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MangaHunter.Infrastructure.Persistence.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateAuditableEntitiesInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = dbContext.ChangeTracker.Entries<IAuditableEntity>();
        foreach (var entry in entries)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Property(a => a.CreatedOnUtc).CurrentValue = _dateTimeProvider.UtcNow;
            }

            if (entry.State is EntityState.Modified)
            {
                entry.Property(a => a.UpdatedOnUtc).CurrentValue = _dateTimeProvider.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}