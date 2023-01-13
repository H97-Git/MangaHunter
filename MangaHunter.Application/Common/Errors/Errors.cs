using ErrorOr;

namespace MangaHunter.Application.Common.Errors;

public static class Errors
{
    public static class Common
    {
        public static Error JsonSerializerFailure => Error.Failure(code: "JsonSerializer.Failure",
            description: "Failed to de/serialize the json resources.");

        public static Error XmlSerializerFailure => Error.Failure(code: "XmlSerializer.Failure",
            description: "Failed to de/serialize the xml resources.");
    }

    public static class Mangadex
    {
        public static Error NotFound => Error.NotFound(code: "Mangadex.NotFound",
            description: "Manga @Mangadex not found.");

        public static Error CollectionEmpty => Error.NotFound(code: "Mangadex.CollectionEmpty",
            description: "No results for this search.");

        public static Error Unexpected => Error.Unexpected(code: "Mangadex.Unexpected",
            description: "Something unexpected happened with the Mangadex service.");
    }

    public static class MangaUpdates
    {
        public static Error NotFound => Error.NotFound(code: "MangaUpdates.NotFound",
            description: "Manga @MangaUpdates not found.");

        public static Error Unexpected => Error.Unexpected(code: "Mangadex.Unexpected",
            description: "Something unexpected happened with the MangaUpdates service.");

        public static Error LinksNull => Error.Custom(405, code: "MangaUpdates.Link.Null",
            description: "No link for MangaUpdates.");

        public static Error SeriesIdNull => Error.Custom(405, code: "MangaUpdates.SeriesId.Null",
            description: "No series ID for MangaUpdates.");
    }

    public static class Hunter
    {
        public static Error NotFound => Error.NotFound(code: "Hunter.NotFound",
            description: "Hunter not found.");

        public static Error Duplicate => Error.Conflict(code: "Hunter.Duplicate",
            description: "Hunter already in user list.");

        public static Error Conflict => Error.Conflict(code: "Hunter.Conflict",
            description: "Some conflict happened with the request");
    }

    public static class TierList
    {
        public static Error NotFound => Error.NotFound(code: "TierList.NotFound",
            description: "TierList not found.");

        public static Error Conflict => Error.Conflict(code: "TierList.Conflict",
            description: "Some conflict happened with the request.");

        public static Error Duplicates => Error.Conflict("TierList.Duplicates",
            description: "Found duplicates in tierlist.");

        public static Error Unexpected => Error.Unexpected("TierList.Update.Unexpected",
                description: "Failed to update the tierlist.");
    }
}