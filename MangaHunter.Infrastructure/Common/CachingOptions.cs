using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

namespace MangaHunter.Infrastructure.Common;

public static class CachingOptions
{
    public static JsonSerializerSettings JsonSerializerSettings => new ()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Ignore,
    };

    private static TimeSpan AbsoluteExpirationRepository => TimeSpan.FromSeconds(5);
    private static TimeSpan AbsoluteExpirationMangadex => TimeSpan.FromHours(1);
    private static TimeSpan AbsoluteExpirationMangadexCover => TimeSpan.FromDays(1);
    private static TimeSpan AbsoluteExpirationMangaUpdates => TimeSpan.FromHours(1);
    private static TimeSpan AbsoluteExpirationMangaUpdatesRss => TimeSpan.FromHours(1);

    public static DistributedCacheEntryOptions CacheEntryOptionsRepository => new()
    {
        SlidingExpiration = AbsoluteExpirationRepository,
    };

    public static DistributedCacheEntryOptions CacheEntryOptionsMangadex => new()
    {
        SlidingExpiration = AbsoluteExpirationMangadex,
    };

    public static DistributedCacheEntryOptions CacheEntryOptionsMangadexCover => new()
    {
        SlidingExpiration = AbsoluteExpirationMangadexCover,
    };

    public static DistributedCacheEntryOptions CacheEntryOptionsMangaUpdates => new()
    {
        SlidingExpiration = AbsoluteExpirationMangaUpdates,
    };

    public static DistributedCacheEntryOptions CacheEntryOptionsMangaUpdatesRss => new()
    {
        SlidingExpiration = AbsoluteExpirationMangaUpdatesRss,
    };
}