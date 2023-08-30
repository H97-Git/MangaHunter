using ErrorOr;

using MangaDexSharpOld;
using MangaDexSharpOld.Collections;
using MangaDexSharpOld.Parameters.Cover;
using MangaDexSharpOld.Parameters.Manga;
using MangaDexSharpOld.Resources;

using MangaDexSharp;

using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Services;

using Serilog;

using Manga = MangaDexSharpOld.Resources.Manga;

namespace MangaHunter.Infrastructure.Services;

public class MangadexService
{
    private const int MaxCount = 100;
    private readonly IMangaDex _md;
    private readonly MangaDexClient _client = new();

    public MangadexService(IMangaDex md)
    {
        this._md = md;
    }

    public async Task<ErrorOr<CoverArt>> GetCover(Guid id)
    {
        try
        {
            return await _client.Cover.GetCover(id, null);
            await _md.Cover.Get(id.ToString());
        }
        catch (Exception e)
        {
            Log.Error(e, "Mangadex Service :");
            return Errors.Mangadex.Unexpected;
        }
    }

    public async Task<ErrorOr<List<CoverArt>>> GetCoverList(List<Guid> coverArtIds)
    {
        try
        {
            ResourceCollection<CoverArt>? collection;
            if (coverArtIds.Count > MaxCount)
            {
                var previousCount = 0;
                var coverArts = new List<CoverArt>();
                while (previousCount < coverArtIds.Count)
                {
                    await Task.Delay(200);
                    var ids = coverArtIds.Skip(previousCount).Take(MaxCount).ToList();
                    if (ids.Count is 0) continue;
                    collection = await _client.Cover.GetList(new GetCoverListParameters
                    {
                        CoverIds = ids, Amount = ids.Count,
                    });
                    // await _md.Cover.List(new CoverArtFilter() {MangaIds = ids});
                    coverArts.AddRange(collection);
                    previousCount += MaxCount;
                }

                return coverArts;
            }

            collection = await _client.Cover.GetList(new GetCoverListParameters
            {
                CoverIds = coverArtIds, Amount = coverArtIds.Count
            });
            return collection.ToList();
        }
        catch (Exception e)
        {
            Log.Error(e, "Mangadex Service :");
            return Errors.Mangadex.Unexpected;
        }
    }

    public async Task<ErrorOr<MangaDexRoot<MangaDexSharp.Manga>>> GetByGuid(Guid mangaId)
    {
        try
        {
            var manga = await this._md.Manga.Get(mangaId.ToString(), new[] {MangaIncludes.cover_art});
            return manga.Result is "ok" ? manga : Errors.Mangadex.NotFound;
        }
        catch (Exception e)
        {
            Log.Error(e, "Mangadex Service :");
            return Errors.Mangadex.Unexpected;
        }
    }


    public async Task<ErrorOr<List<Manga>>> GetListByGuids(List<Guid> mangaIds)
    {
        Log.Debug("MangadexService GetListByGuids()");
        try
        {
            ResourceCollection<Manga>? collection;
            if (mangaIds.Count > MaxCount)
            {
                var previousCount = 0;
                var mangas = new List<Manga>();
                while (previousCount < mangaIds.Count)
                {
                    await Task.Delay(200);
                    var ids = mangaIds.Skip(previousCount).Take(MaxCount).ToList();
                    if (ids.Count is 0) continue;
                    collection =
                        await _client.Manga.GetList(new GetMangaListParameters {MangaIds = ids, Amount = ids.Count});
                    mangas.AddRange(collection);
                    previousCount += MaxCount; // Should be collection.count ?
                }

                return mangas;
            }

            collection = await _client.Manga.GetList(new GetMangaListParameters
            {
                MangaIds = mangaIds, Amount = mangaIds.Count
            });
            return collection.ToList();
        }
        catch (Exception e)
        {
            Log.Error(e, "Mangadex Service :");
            return Errors.Mangadex.Unexpected;
        }
    }

    public async Task<ErrorOr<List<Manga>>> Search(GetMangaListParameters mangaListParameters)
    {
        ResourceCollection<Manga> resourceCollection;
        try
        {
            resourceCollection = await _client.Manga.GetList(mangaListParameters);
            if (resourceCollection.Count is 0)
            {
                return Errors.Mangadex.CollectionEmpty;
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "Mangadex Service : Search");
            return Errors.Mangadex.Unexpected;
        }

        return resourceCollection.ToList();
    }
}