using ErrorOr;

using MangaHunter.BlazorServer.Common.Services.HttpClients;
using MangaHunter.Contracts.Common;
using MangaHunter.Contracts.Hunter;
using MangaHunter.Contracts.TierList;

namespace MangaHunter.BlazorServer.Common.Services;

public interface IApiService
{
    Task<ErrorOr<List<HunterResponse>>> GetUserList(string username, QueryParameters parameters);
    Task<ErrorOr<HunterResponseWithPagination>> GetUserListWithPagination(string username, QueryParameters parameters);
    Task<ErrorOr<HunterResponseNew>> GetMangaById(string username, string mangadexId, QueryParameters parameters);
    Task<ErrorOr<long>> GetNewMangaUpdatesId(string username, string mangadexId);
    Task<ErrorOr<HunterDto>> AddManga(string username, HunterDto hunterDto);
    Task<ErrorOr<HunterDto>> UpdateManga(string username, HunterDto hunterDto);
    Task<ErrorOr<Deleted>> DeleteManga(string username, string mangadexId);
    Task<ErrorOr<List<HunterResponseNew>>> Search(SearchQueryParameters parameters);
    Task<ErrorOr<TodayRssDto>> GetTodayRss();
    Task<ErrorOr<List<TierListDto>>> GetUserTierList(string username);
    Task<ErrorOr<TierListResponse>> GetTierListById(string code);
    Task<ErrorOr<TierListDto>> AddTierList(string username, TierListDto tierListModel);
    Task<ErrorOr<TierListDto>> UpdateTierList(string username, TierListDto tierListModel);
    Task<ErrorOr<Deleted>> DeleteTierList(string username, int id);
}

public class ApiService : IApiService
{
    private readonly IApiClient _apiClient;

    public ApiService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ErrorOr<TodayRssDto>> GetTodayRss()
    {
        return await _apiClient.GetRequest<TodayRssDto>("/rss/today");
    }

    public async Task<ErrorOr<List<HunterResponse>>> GetUserList(string username, QueryParameters parameters)
    {
        var hasMangadex = $"hasMangadex={parameters.HasMangadex}";
        var hasMangaUpdates = $"hasMangaUpdates={parameters.HasMangaUpdates}";
        var hasMangaUpdatesRss = $"hasMangaUpdatesRss={parameters.HasMangaUpdatesRss}";

        var uri = $"user/{username}/hunter/all?{hasMangadex}&{hasMangaUpdates}&{hasMangaUpdatesRss}";
        return await _apiClient.GetRequest<List<HunterResponse>>(uri);
    }

    public async Task<ErrorOr<HunterResponseWithPagination>> GetUserListWithPagination(string username,
        QueryParameters parameters)
    {
        var pageParams = $"page={parameters.Page}";
        var sizeParams = $"size={parameters.Size}";
        var statusParams = $"status={parameters.Status}";
        var hasMangadex = $"hasmangadex={parameters.HasMangadex}";
        var hasMangaUpdates = $"hasmangaupdates={parameters.HasMangaUpdates}";
        var hasMangaUpdatesRss = $"hasmangaupdatesrss={parameters.HasMangaUpdatesRss}";
        var excludedStatus = $"excludedstatus={parameters.ExcludedStatus}";

        var uri =
            $"user/{username}/hunter?{pageParams}&{sizeParams}&{statusParams}&{excludedStatus}&{hasMangadex}&{hasMangaUpdates}&{hasMangaUpdatesRss}";
        return await _apiClient.GetRequest<HunterResponseWithPagination>(uri);
    }

    public async Task<ErrorOr<HunterResponseNew>> GetMangaById(string userId, string mangadexId,
        QueryParameters parameters)
    {
        var hasMangadex = $"hasMangadex={parameters.HasMangadex}";
        var hasMangaUpdates = $"hasMangaUpdates={parameters.HasMangaUpdates}";
        var hasMangaUpdatesRss = $"hasMangaUpdatesRss={parameters.HasMangaUpdatesRss}";

        return await _apiClient
            .GetRequest<HunterResponseNew>(
                $"user/{userId}/Hunter/{mangadexId}?{hasMangadex}&{hasMangaUpdates}&{hasMangaUpdatesRss}");
    }

    public async Task<ErrorOr<long>> GetNewMangaUpdatesId(string userId, string mangadexId)
    {
        return await _apiClient
            .GetRequest<long>($"Hunter/mangaupdatesid/{mangadexId}");
    }

    public async Task<ErrorOr<HunterDto>> AddManga(string userId, HunterDto hunterDto)
    {
        return await _apiClient
            .PostRequest($"user/{userId}/Hunter/add", hunterDto);
    }

    public async Task<ErrorOr<HunterDto>> UpdateManga(string userId, HunterDto hunterDto)
    {
        return await _apiClient
            .PutRequest($"user/{userId}/Hunter/edit/{hunterDto.MangadexId}", hunterDto);
    }

    public async Task<ErrorOr<Deleted>> DeleteManga(string userId, string mangadexId)
    {
        return await _apiClient
            .DeleteRequest($"user/{userId}/Hunter/delete/{mangadexId}");
    }

    public async Task<ErrorOr<List<HunterResponseNew>>> Search(SearchQueryParameters parameters)
    {
        return await _apiClient
            .SearchRequest("search", parameters);
    }

    public async Task<ErrorOr<List<TierListDto>>> GetUserTierList(string username)
    {
        return await _apiClient
            .GetRequest<List<TierListDto>>($"user/{username}/TierList/all");
    }

    public async Task<ErrorOr<TierListResponse>> GetTierListById(string code)
    {
        return await _apiClient
            .GetRequest<TierListResponse>($"TierList/{code}");
    }

    public async Task<ErrorOr<TierListDto>> AddTierList(string username, TierListDto tierListModel)
    {
        return await _apiClient
            .PostRequest($"user/{username}/TierList/add", tierListModel);
    }

    public async Task<ErrorOr<TierListDto>> UpdateTierList(string username, TierListDto tierListModel)
    {
        return await _apiClient
            .PutRequest($"user/{username}/TierList/edit/{tierListModel.Id}", tierListModel);
    }

    public async Task<ErrorOr<Deleted>> DeleteTierList(string username, int id)
    {
        return await _apiClient
            .DeleteRequest($"user/{username}/TierList/delete/{id}");
    }
}