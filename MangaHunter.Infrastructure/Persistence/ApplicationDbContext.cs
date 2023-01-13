using MangaHunter.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Newtonsoft.Json;

namespace MangaHunter.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var guidsConverter = new ValueConverter<ICollection<Guid>, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<List<Guid>>(v) ?? new List<Guid>());
        var guidsValueComparer = new ValueComparer<ICollection<Guid>>(
            (c1, c2) => c2 != null && c1 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        modelBuilder.Entity<TierList>()
            .Property(e => e.UnlistedMangaIds)
            .HasConversion(guidsConverter, guidsValueComparer);
        modelBuilder.Entity<TierRow>()
            .Property(e => e.MangaIds)
            .HasConversion(guidsConverter, guidsValueComparer);
    }

    public DbSet<Hunter> Hunters { get; set; }
    public DbSet<TierList> TierLists { get; set; }
    // public DbSet<TierRow> TierRow { get; set; }
}