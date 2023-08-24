﻿namespace MangaHunter.Contracts.Mangadex;

using Models.Relationships;
using Models.ScanlationGroup;
using Models.Types;
using Models.User;

/// <summary>
/// A bunch of useful extensions for MD related tasks
/// </summary>
public static class Extensions
{
    /// <summary>
    /// An array of all of the available content ratings
    /// </summary>
    public static ContentRating[] ContentRatingsAll => new[]
    {
        ContentRating.safe, ContentRating.erotica, ContentRating.suggestive, ContentRating.pornographic
    };


    /// <summary>
    /// Fetches all of a specific type of related item
    /// </summary>
    /// <typeparam name="T">The type of relationship data to fetch</typeparam>
    /// <param name="source">Where to fetch the relationship data from</param>
    /// <returns>The related items</returns>
    public static T[] Relationship<T>(this IRelationshipModel? source) where T : IRelationship
    {
        return source?
            .Relationships
            .Where(t => t is T)
            .Select(t => (T)t)
            .ToArray() ?? Array.Empty<T>();
    }

    /// <summary>
    /// Fetches all of the cover art from the given source
    /// </summary>
    /// <param name="source">The relationship source</param>
    /// <returns>All of the related cover art</returns>
    public static CoverArtRelationship[] CoverArt(this IRelationshipModel? source) =>
        source.Relationship<CoverArtRelationship>();

    /// <summary>
    /// Fetches all of the people from the given source
    /// </summary>
    /// <param name="source">The relationship source</param>
    /// <returns>All of the related people</returns>
    public static PersonRelationship[] People(this IRelationshipModel? source) =>
        source.Relationship<PersonRelationship>();

    /// <summary>
    /// Fetches all of the related manga from the given source
    /// </summary>
    /// <param name="source">The relationship source</param>
    /// <returns>All of the related manga</returns>
    public static RelatedDataRelationship[] Manga(this IRelationshipModel? source) =>
        source.Relationship<RelatedDataRelationship>();

    /// <summary>
    /// Fetches all of the related scanlation groups from the given source
    /// </summary>
    /// <param name="source">The relationship source</param>
    /// <returns>All of the related scanlation groups</returns>
    public static ScanlationGroup[] ScanlationGroups(this IRelationshipModel? source) =>
        source.Relationship<ScanlationGroup>();

    /// <summary>
    /// Fetches all of the related users from the given source
    /// </summary>
    /// <param name="source">The relationship source</param>
    /// <returns>All of the related users</returns>
    public static User[] Users(this IRelationshipModel? source) => source.Relationship<User>();
}