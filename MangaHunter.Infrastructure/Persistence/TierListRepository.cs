using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace MangaHunter.Infrastructure.Persistence;

public class TierListRepository : ITierListRepository
{
    private static ApplicationDbContext _dbContext;
    private static DbSet<TierList> _tierList;

    public TierListRepository(ApplicationDbContext dataContext)
    {
        _dbContext = dataContext;
        _tierList = dataContext.TierLists;
    }

    public async Task<List<TierList>> GetByUsername(string username)
    {
        return await _tierList
            .AsNoTracking()
            .Include(x => x.Tiers)
            .Where(x => x.UserName == username)
            .ToListAsync();
    }

    public Task<TierList?> GetByIdNoCaching(int id)
        => GetById(id);

    public async Task<TierList?> GetById(int id)
    {
        return await _tierList
            .AsNoTracking()
            .Include(x => x.Tiers)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public bool IsShareCodeNew(string shareCode) =>
        !_tierList.Any(x => x.ShareCode == shareCode);

    public async Task<TierList?> GetByShareCode(string shareCode)
    {
        return await _tierList
            .AsNoTracking()
            .Include(x => x.Tiers)
            .FirstOrDefaultAsync(x => x.ShareCode == shareCode);
    }

    public async Task<TierList> Add(TierList tierList)
    {
        await _tierList.AddAsync(tierList);
        await SaveChangesAsync();
        return tierList;
    }

    public async Task<Updated> Update(TierList tierList)
    {
        _tierList.Update(tierList);
        await SaveChangesAsync();
        return new Updated();
    }

    public async Task<Deleted> Delete(TierList tierList)
    {
        _tierList.Remove(tierList);
        await SaveChangesAsync();
        return new Deleted();
    }

    public void DeleteRow(TierRow row)
    {
        _dbContext.Entry(row).State = EntityState.Deleted;
    }

    private static async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}