namespace MangaHunter.Domain.Primitives;

public interface IAuditableEntity
{
    DateTime CreatedOnUtc { get; set; }
    DateTime? UpdatedOnUtc { get; set; }
}