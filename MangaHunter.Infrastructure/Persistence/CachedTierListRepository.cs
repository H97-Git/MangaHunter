using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Domain.Entities;
using MangaHunter.Infrastructure.Common;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

namespace MangaHunter.Infrastructure.Persistence;

public class CachedTierListRepository : ITierListRepository
{
    private readonly TierListRepository _decorated;
    private readonly IDistributedCache _distributedCache;

    public CachedTierListRepository(TierListRepository decorated,
        IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }

    public async Task<List<TierList>> GetByUsername(string username)
    {
        string key = $"tierlists.username.{username}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<List<TierList>>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        List<TierList> tierLists = await _decorated.GetByUsername(username);
        if (tierLists is {Count: 0})
        {
            return tierLists;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(tierLists, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsRepository);
        return tierLists;
    }

    public Task<TierList?> GetByIdNoCaching(int id)
        => _decorated.GetByIdNoCaching(id);


    public async Task<TierList?> GetById(int id)
    {
        string key = $"tierlist.id.{id}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<TierList>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        TierList? tierLists = await _decorated.GetById(id);
        if (tierLists is null)
        {
            return tierLists;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(tierLists, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsRepository);
        return tierLists;
    }


    public async Task<TierList?> GetByShareCode(string shareCode)
    {
        string key = $"tierlist.sharecode.{shareCode}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<TierList>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        TierList? tierLists = await _decorated.GetByShareCode(shareCode);
        if (tierLists is null)
        {
            return tierLists;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(tierLists, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsRepository);
        return tierLists;
    }

    public bool IsShareCodeNew(string shareCode)
        => _decorated.IsShareCodeNew(shareCode);

    public Task<TierList> Add(TierList tierList)
        => _decorated.Add(tierList);

    public Task<Updated> Update(TierList tierList)
        => _decorated.Update(tierList);

    public Task<Deleted> Delete(TierList tierList)
        => _decorated.Delete(tierList);

     public void DeleteRow(TierRow row)
         => _decorated.DeleteRow(row);
}