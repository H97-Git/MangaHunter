using ErrorOr;

using MangaHunter.Application.Common;
using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Infrastructure.Common;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using Serilog;

namespace MangaHunter.Infrastructure.Services;

public class CachedMangaUpdatesService : IMangaUpdatesService
{
    private readonly MangaUpdatesService _decorated;
    private readonly IDistributedCache _distributedCache;


    public CachedMangaUpdatesService(MangaUpdatesService decorated,
        IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }


    public async Task<ErrorOr<MangaUpdates>> GetMangaUpdatesUnsafe(string? linksMangaUpdates)
    {
        string key = $"unsafe-{linksMangaUpdates}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert.DeserializeObject<MangaUpdates>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Log.Debug($"Get a new value for {key}");

        ErrorOr<MangaUpdates> mangaUpdates = await _decorated.GetMangaUpdatesUnsafe(linksMangaUpdates);
        if (mangaUpdates.IsError)
        {
            return mangaUpdates;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(mangaUpdates.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangaUpdates);
        return mangaUpdates.Value;
    }

    public async Task<ErrorOr<MangaUpdates>> GetMangaUpdatesSafe(string? seriesId)
    {
        string key = $"safe-{seriesId}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert.DeserializeObject<MangaUpdates>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Log.Debug($"Get a new value for {key}");

        ErrorOr<MangaUpdates> mangaUpdates = await _decorated.GetMangaUpdatesSafe(seriesId);
        if (mangaUpdates.IsError)
        {
            return mangaUpdates;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(mangaUpdates.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangaUpdates);
        return mangaUpdates.Value;
    }

    public async Task<ErrorOr<RssMangaUpdates>> GetRss(string? seriesId)
    {
        string key = $"rss-{seriesId}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert.DeserializeObject<RssMangaUpdates>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Log.Debug($"Get a new value for {key}");

        ErrorOr<RssMangaUpdates> rss = await _decorated.GetRss(seriesId);
        if (rss.IsError)
        {
            return rss;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(rss.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangaUpdatesRss);
        return rss.Value;
    }
    
    public async Task<ErrorOr<TodayRss>> GetTodayRss()
    {
        const string key = "today-rss";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert.DeserializeObject<TodayRss>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Log.Debug($"Get a new value for {key}");

        var todayRss = await _decorated.GetTodayRss();
        if (todayRss.IsError)
        {
            return todayRss;
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(todayRss.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangaUpdatesRss);
        return todayRss.Value;
    }
}