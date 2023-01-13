using MangaHunter.Application.Common.Interfaces.Services;

namespace MangaHunter.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}