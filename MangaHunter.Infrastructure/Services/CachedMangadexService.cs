using ErrorOr;

using MangaDexSharpOld.Parameters.Manga;
using MangaDexSharpOld.Resources;

using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Domain.Common;
using MangaHunter.Infrastructure.Common;

using Mapster;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using Serilog;

namespace MangaHunter.Infrastructure.Services;

public class CachedMangadexService : IMangadexService
{
    private readonly MangadexService _decorated;
    private readonly IDistributedCache _distributedCache;

    public CachedMangadexService(MangadexService decorated,
        IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }


    public async Task<ErrorOr<CoverArtSerializable>> GetCover(Guid mangaId)
    {
        string key = $"cover.{mangaId}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert.DeserializeObject<CoverArtSerializable>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Log.Debug($"Get a new value for {key}");
        ErrorOr<CoverArt> coverArt = await _decorated.GetCover(mangaId);
        if (coverArt.IsError)
        {
            return coverArt.Adapt<CoverArtSerializable>();
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(coverArt.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangadexCover);
        return coverArt.Value.Adapt<CoverArtSerializable>();
    }

    public async Task<ErrorOr<List<CoverArtSerializable>>> GetCoverList(List<Guid> coverArtIds)
    {
        string key = CreateKeyFromLists("coverList", coverArtIds);
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return
                JsonConvert.DeserializeObject<List<CoverArtSerializable>>(cached, CachingOptions.JsonSerializerSettings)
                !;
        }

        Log.Debug($"Get a new value for {key}");
        ErrorOr<List<CoverArt>> coverList = await _decorated.GetCoverList(coverArtIds);
        if (coverList.IsError)
        {
            return coverList.Adapt<List<CoverArtSerializable>>();
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(coverList.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangadexCover);
        return coverList.Value.Adapt<List<CoverArtSerializable>>();
    }

    public async Task<ErrorOr<MangaDexSharp.Manga>> GetByGuid(Guid mangaId)
    {
        string key = $"mangadex.{mangaId}";
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert.DeserializeObject<MangaDexSharp.Manga>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Log.Debug($"Get a new value for {key}");
        ErrorOr<MangaDexSharp.Manga> manga = await _decorated.GetByGuid(mangaId);
        if (manga.IsError)
        {
            // return manga.Adapt<MangaSerializable>();
            return new ErrorOr<MangaDexSharp.Manga>();
        }

        var json = JsonConvert.SerializeObject(manga.Value, CachingOptions.JsonSerializerSettings);
        await _distributedCache.SetStringAsync(key, json, CachingOptions.CacheEntryOptionsMangadex);

        return manga;
    }

    public async Task<ErrorOr<List<MangaSerializable>>> GetListByGuids(List<Guid> mangaIds)
    {
        string key = CreateKeyFromLists("mangadexList", mangaIds);
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert.DeserializeObject<List<MangaSerializable>>(cached, CachingOptions.JsonSerializerSettings)
                !;
        }

        Log.Debug($"Get a new value for {key}");
        ErrorOr<List<Manga>> mangas = await _decorated.GetListByGuids(mangaIds);
        if (mangas.IsError)
        {
            return mangas.Adapt<List<MangaSerializable>>();
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(mangas.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangadex);
        return mangas.Value.Adapt<List<MangaSerializable>>();
    }

    public async Task<ErrorOr<List<MangaSerializable>>> Search(GetMangaListParameters mangaListParameters)
    {
        string keyName = $"search.{mangaListParameters.Title}.{mangaListParameters.Amount}";
        string key = CreateKeyFromLists(keyName, (mangaListParameters.IncludeTags?.ToList()),
            mangaListParameters.ExcludeTags?.ToList());
        var cached = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            Log.Debug($"key {key} found in cache");
            return JsonConvert
                .DeserializeObject<List<MangaSerializable>>(cached, CachingOptions.JsonSerializerSettings)!;
        }

        Log.Debug($"Get a new value for {key}");
        ErrorOr<List<Manga>> mangas = await _decorated.Search(mangaListParameters);
        if (mangas.IsError)
        {
            return mangas.Adapt<List<MangaSerializable>>();
        }

        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(mangas.Value, CachingOptions.JsonSerializerSettings),
            CachingOptions.CacheEntryOptionsMangadex);
        return mangas.Value.Adapt<List<MangaSerializable>>();
    }

    private static string CreateKeyFromLists(string keyName, List<Guid>? guids1, List<Guid>? guids2 = null)
    {
        string aggregate = String.Empty;
        if (guids1 is not null or {Count: 0})
        {
            var chars1 = guids1.ConvertAll(c => c.ToString()[..8]);
            aggregate = chars1.OrderBy(x => x).Aggregate(string.Empty, (current, str) => current + str);
        }

        if (guids2 is not null or {Count: 0})
        {
            var chars2 = guids2.ConvertAll(c => c.ToString()[..8]);
            aggregate = chars2.Aggregate(aggregate, (current, str) => current + str);
        }

        return $"{keyName}.{aggregate}";
    }
}