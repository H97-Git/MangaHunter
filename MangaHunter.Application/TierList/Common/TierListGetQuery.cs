using MangaHunter.Application.Common.Interfaces.Services;

namespace MangaHunter.Application.TierList.Common;

using ErrorOr;

public static class TierListGetQuery
{
    public static async Task<ErrorOr<TierListResult>> Handle(IMangadexService mangadex,
        Domain.Entities.TierList? tierlist)
    {
        if (tierlist is null)
        {
            return Error.NotFound();
        }

        var allIds = tierlist.AllIdsInTierList;
        if (allIds.Count is 0)
        {
            return new TierListResult {TierList = tierlist};
        }

        var mangas = await mangadex.GetListByGuids(allIds);
        if (mangas.IsError)
        {
            return mangas.FirstError;
        }

        // This is a safe check but it shouldn't happen
        if (mangas.Value.Count is 0)
        {
            return new TierListResult {TierList = tierlist};
        }

        var coverArts = await mangadex.GetCoverList(mangas.Value.ConvertAll(x => x.MainCoverArtId));
        return coverArts.IsError
            ? coverArts.FirstError
            : new TierListResult {TierList = tierlist, Mangas = mangas.Value, CoverArts = coverArts.Value};
    }
}