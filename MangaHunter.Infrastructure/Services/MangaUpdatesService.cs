using System.Net;
using System.Xml.Serialization;

using ErrorOr;

using MangaHunter.Application.Common;
using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Services;

using Newtonsoft.Json;

using Serilog;

namespace MangaHunter.Infrastructure.Services;

public class MangaUpdatesService : IMangaUpdatesService
{
    private readonly HttpClient _client;
    private readonly HttpClient _scraper;

    public MangaUpdatesService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient();
        _scraper = factory.CreateClient();
        _client.BaseAddress = new Uri("https://api.mangaupdates.com/v1/");
    }

    private const string ReplaceUri = "https://www.mangaupdates.com/series.html?id=";
    private static string GetIdFromUri(string uri) => uri.Replace(ReplaceUri, "");
    private static string SeriesUri(string id) => $"series/{id}";
    private static string RssUri(string id) => $"{SeriesUri(id)}/rss";


    public async Task<ErrorOr<MangaUpdates>> GetMangaUpdatesUnsafe(string? linksMangaUpdates)
    {
        if (string.IsNullOrEmpty(linksMangaUpdates))
        {
            return Errors.MangaUpdates.LinksNull;
        }

        var id = GetIdFromUri(linksMangaUpdates);
        var newId = await ScrapeNewId(id);

        var responseMessage = string.IsNullOrEmpty(newId)
            ? await _client.GetAsync(SeriesUri(Decode(id)))
            : await _client.GetAsync(SeriesUri(newId));

        if (responseMessage.StatusCode is HttpStatusCode.NotFound)
        {
            return Errors.MangaUpdates.NotFound;
        }

        if (!responseMessage.IsSuccessStatusCode)
        {
            return Errors.MangaUpdates.Unexpected;
        }

        var response = await responseMessage.Content.ReadAsStringAsync();
        var deserialize = JsonConvert.DeserializeObject<MangaUpdates>(response);
        return deserialize is null
            ? Errors.Common.JsonSerializerFailure
            : deserialize;
    }

    public async Task<ErrorOr<MangaUpdates>> GetMangaUpdatesSafe(string? seriesId)
    {
        if (string.IsNullOrEmpty(seriesId))
        {
            return Errors.MangaUpdates.SeriesIdNull;
        }

        Log.Debug("MangaUpdatesService GetMangaUpdatesSafe()");
        var seriesUri = SeriesUri(seriesId);
        var responseMessage = await _client.GetAsync(seriesUri);
        if (responseMessage.StatusCode is HttpStatusCode.NotFound)
        {
            return Errors.MangaUpdates.NotFound;
        }

        if (!responseMessage.IsSuccessStatusCode)
        {
            return Errors.MangaUpdates.Unexpected;
        }

        var response = await responseMessage.Content.ReadAsStringAsync();
        var deserialize = JsonConvert.DeserializeObject<MangaUpdates>(response);
        return deserialize is null ? Errors.Common.JsonSerializerFailure : deserialize;
    }

    public async Task<ErrorOr<RssMangaUpdates>> GetRss(string? seriesId)
    {
        if (string.IsNullOrEmpty(seriesId))
        {
            return Errors.MangaUpdates.LinksNull;
        }

        Log.Debug("MangaUpdatesService GetRss()");
        var responseMessage = await _client.GetAsync(RssUri(seriesId));
        var content = await responseMessage.Content.ReadAsStringAsync();
        return content.FromXml<RssMangaUpdates>();
    }

    public async Task<ErrorOr<TodayRss>> GetTodayRss()
    {
        Log.Debug("MangaUpdatesService GetTodayRss()");
        var responseMessage = await _client.GetAsync("releases/days");
        var content = await responseMessage.Content.ReadAsStringAsync();
        var deserialize =  JsonConvert.DeserializeObject<TodayRss>(content);
        return deserialize!;
    }

    private static string Decode(string input)
    {
        const string charList = "0123456789abcdefghijklmnopqrstuvwxyz";
        var reversed = input.ToLower().Reverse();
        long result = 0;
        var pos = 0;
        foreach (var c in reversed)
        {
            result += charList.IndexOf(c) * (long)Math.Pow(36, pos);
            pos++;
        }

        return result.ToString();
    }

    private async Task<string> ScrapeNewId(string id)
    {
        const string start = "https://api.mangaupdates.com/v1/series/";
        const string end = "/rss";
        var responseMessage = await _scraper.GetAsync(ReplaceUri + id);
        var newId = await responseMessage.Content.ReadAsStringAsync();

        var indexOfStart = newId.IndexOf(start, StringComparison.Ordinal);
        if (indexOfStart is -1)
        {
            //Can't find "start" we should be in the wrong place meaning we should decode the Id ?
            return string.Empty;
        }

        newId = newId[(indexOfStart + start.Length)..];

        var indexOfEnd = newId.IndexOf(end, StringComparison.Ordinal);
        newId = newId[..(indexOfEnd)];
        return newId;
    }
}

public static class XmlHelper
{
    public static ErrorOr<T> FromXml<T>(this string value)
    {
        using TextReader reader = new StringReader(value);
        try
        {
            var xml = (T?)new XmlSerializer(typeof(T)).Deserialize(reader);
            return xml is null
                ? Errors.Common.XmlSerializerFailure
                : xml;
        }
        catch (Exception e)
        {
            Log.Error(e, "XmlSerializerFailure");
            return Errors.Common.XmlSerializerFailure;
        }
    }
}