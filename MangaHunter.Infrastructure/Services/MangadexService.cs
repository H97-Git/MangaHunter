using ErrorOr;

using MangaDexSharp;
using MangaDexSharp.Collections;
using MangaDexSharp.Parameters.Cover;
using MangaDexSharp.Parameters.Manga;
using MangaDexSharp.Resources;

using MangaHunter.Application.Common.Errors;

using Serilog;

namespace MangaHunter.Infrastructure.Services;

public class MangadexService
{
    private readonly MangaDexClient _client = new();

    private const int MaxCount = 100;

    public async Task<ErrorOr<CoverArt>> GetCover(Guid id)
    {
        try
        {
            return await _client.Cover.GetCover(id, null);
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

    public async Task<ErrorOr<Manga>> GetByGuid(Guid mangaId)
    {
        try
        {
            var manga = await _client.Manga.ViewManga(mangaId);
            return manga;
            //Errors.Mangadex.NotFound
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