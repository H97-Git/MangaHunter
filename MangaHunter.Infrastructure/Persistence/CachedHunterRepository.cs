using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Hunter;
using MangaHunter.Domain.Common;
using MangaHunter.Domain.Entities;
using MangaHunter.Infrastructure.Common;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

namespace MangaHunter.Infrastructure.Persistence;

public class CachedHunterRepository : IHunterRepository
{
    private readonly HunterRepository _decorated;
    private readonly IDistributedCache _distributedCache;

    public CachedHunterRepository(HunterRepository decorated,
        IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }
    
    public async Task<List<Hunter>> GetByUsername(string username)
    {
        string key = $"hunters.{username}";
        var cached = await _distributedCache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<List<Hunter>>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        List<Hunter> list = await _decorated.GetByUsername(username);
        if (list is {Count: 0})
        {
            return list;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(list, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsRepository);
        return list;
    }

    public async Task<HunterResultWithPagination> GetByUsernameWithPagination(string username,
        PaginationParameters parameters)
    {
        string key = $"hunters.{username}.{parameters.Size}.{parameters.Page}.{parameters.Status}.{parameters.ExcludedStatus}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<HunterResultWithPagination>(cached,
                CachingOptions.JsonSerializerSettings)!;
        }

        HunterResultWithPagination resultWithPagination =
            await _decorated.GetByUsernameWithPagination(username, parameters);
        if (resultWithPagination.Total is 0)
        {
            return resultWithPagination;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(resultWithPagination, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsRepository);
        return resultWithPagination;
    }

    public async Task<Hunter?> GetByMangaId(Guid mangadexId, string username)
    {
        string key = $"hunter.{username}.{mangadexId}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<Hunter>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Hunter? hunter = await _decorated.GetByMangaId(mangadexId, username);
        if (hunter is null)
        {
            return hunter;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(hunter, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsRepository);
        return hunter;
    }

    public Task<Hunter?> GetByMangaIdNoCaching(Guid mangadexId, string username)
        => _decorated.GetByMangaIdNoCaching(mangadexId, username);

    public async Task<long?> GetMangaUpdatesId(Guid mangadexId)
    {
        string key = $"mangaupdatesid.{mangadexId}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<long>(cached)!;
        }

        long? seriesId = await _decorated.GetMangaUpdatesId(mangadexId);
        if (seriesId is null)
        {
            return seriesId;
        }

        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(seriesId), CachingOptions.CacheEntryOptionsRepository);
        return seriesId;
    }

    public Task<Hunter> Add(Hunter hunter)
        => _decorated.Add(hunter);

    public Task<Hunter> Update(Hunter hunter)
        => _decorated.Update(hunter);

    public Task<Deleted> Delete(Hunter hunter)
        => _decorated.Delete(hunter);
}