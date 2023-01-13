using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Hunter;
using MangaHunter.Domain.Common;
using MangaHunter.Domain.Entities;
using MangaHunter.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace MangaHunter.Infrastructure.Persistence;

public class HunterRepository : IHunterRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<Hunter> _hunters;

    public HunterRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _hunters = dbContext.Hunters;
    }

    public async Task<List<Hunter>> GetByUsername(string username)
    {
        return await _hunters
            .AsNoTracking()
            .Include(h => h.History)
            .Where(h => h.Username == username)
            .ToListAsync();
    }

    public async Task<HunterResultWithPagination> GetByUsernameWithPagination(string username,
        PaginationParameters parameters)
    {
        // Get all user Entity with join(?) 
        var mangas = await _hunters
            .AsNoTracking()
            .Include(x => x.History)
            .Where(x => x.Username == username)
            .OrderByDescending(x => x.CreatedOnUtc)
            .ThenByDescending(x => x.UpdatedOnUtc)
            .ToListAsync();
        var total = mangas.Count;
        var filteredCount = 0;
        // Try to parse a status from parameters
        var status = StringToReadingStatus(parameters.Status);
        // If a status is issued filter the list
        if (status is not null)
        {
            mangas = parameters.ExcludedStatus switch
            {
                true => mangas.Where(x => x.Status != status).ToList(),
                false => mangas.Where(x => x.Status == status).ToList()
            };
            filteredCount = mangas.Count;
        }

        // After filter, paginated the result
        mangas = mangas
            .Skip((parameters.Page - 1) * parameters.Size)
            .Take(parameters.Size)
            .ToList();
        return new HunterResultWithPagination
        {
            Total = total, Filtered = filteredCount, List = mangas.ConvertAll(h => new HunterResult {Hunter = h})
        };
    }

    public async Task<long?> GetMangaUpdatesId(Guid mangadexId)
    {
        var hunter = await _hunters.AsNoTracking()
            .FirstOrDefaultAsync(h => h.MangadexId.Equals(mangadexId) && h.BakaId != 0 && h.BakaId != null);
        return hunter?.BakaId;
    }

    public Task<Hunter?> GetByMangaIdNoCaching(Guid mangadexId, string username)
        => GetByMangaId(mangadexId, username);

    public async Task<Hunter?> GetByMangaId(Guid mangadexId, string userId)
    {
        return await _hunters
            .AsNoTracking()
            .Include(h => h.History)
            .FirstOrDefaultAsync(h => h.MangadexId == mangadexId && h.Username == userId);
    }

    public async Task<Hunter> Add(Hunter hunter)
    {
        await _hunters.AddAsync(hunter);
        await SaveChangesAsync();
        return hunter;
    }

    public async Task<Hunter> Update(Hunter hunter)
    {
        _hunters.Update(hunter);
        await SaveChangesAsync();
        return hunter;
    }

    public async Task<Deleted> Delete(Hunter hunter)
    {
        _hunters.Remove(hunter);
        await SaveChangesAsync();
        return Result.Deleted;
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    private static ReadingStatus? StringToReadingStatus(string? status)
    {
        return status switch
        {
            "wanttoread" => ReadingStatus.WantToRead,
            "reading" => ReadingStatus.Reading,
            "stalled" => ReadingStatus.Reading,
            "read" => ReadingStatus.Read,
            "dropped" => ReadingStatus.Dropped,
            _ => null
        };
    }
}