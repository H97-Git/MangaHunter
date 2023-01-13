using System.Net;
using System.Text;

using ErrorOr;

using MangaHunter.Contracts.Hunter;

using Newtonsoft.Json;

namespace MangaHunter.BlazorServer.Common.Services.HttpClients;

public interface IApiClient
{
    Task<ErrorOr<T>> GetRequest<T>(string uri);
    Task<ErrorOr<T>> PostRequest<T>(string uri, T? args);
    Task<ErrorOr<List<HunterResponse>>> SearchRequest(string uri, SearchQueryParameters parameters);
    Task<ErrorOr<T>> PutRequest<T>(string uri, T? args);
    Task<ErrorOr<Deleted>> DeleteRequest(string uri);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _client;
    private readonly NavigationManagerHandler _navigationManagerHandler;

    public ApiClient(IHttpClientFactory factory, NavigationManagerHandler navigationManagerHandler,
        IHostEnvironment environment)
    {
        _navigationManagerHandler = navigationManagerHandler;
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri(!environment.IsDevelopment() ? "http://api/" : "http://localhost:4000/");
    }

    // Todo : Learn cancellationToken
    public async Task<ErrorOr<T>> GetRequest<T>(string uri)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        return await SendHandleResponse<T>(httpRequestMessage);
    }

    public async Task<ErrorOr<T>> PostRequest<T>(string uri, T? args)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(JsonConvert.SerializeObject(args), Encoding.UTF8,
                "application/json")
        };
        return await SendHandleResponse<T>(httpRequestMessage);
    }

    public async Task<ErrorOr<List<HunterResponse>>> SearchRequest(string uri, SearchQueryParameters parameters)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8,
                "application/json")
        };
        return await SendHandleResponse<List<HunterResponse>>(httpRequestMessage);
    }

    public async Task<ErrorOr<T>> PutRequest<T>(string uri, T? args)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
        {
            Content = new StringContent(JsonConvert.SerializeObject(args), Encoding.UTF8,
                "application/json")
        };
        return await SendHandleResponse<T>(httpRequestMessage);
    }

    public async Task<ErrorOr<Deleted>> DeleteRequest(string uri)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
        return await SendHandleDeleteRequest(httpRequestMessage);
    }

    private async Task<ErrorOr<Deleted>> SendHandleDeleteRequest(HttpRequestMessage httpRequestMessage)
    {
        using var httpResponseMessage = await _client.SendAsync(httpRequestMessage);
        // if (_navigationManagerHandler.UnauthorizedHandler(httpResponseMessage.StatusCode))
        //     return Error.Custom(401, "Unauthorized", "Try to log in again");
        return httpResponseMessage.StatusCode is HttpStatusCode.NoContent
            ? Result.Deleted
            : Error.Failure(description: httpResponseMessage.ReasonPhrase ??
                                         await httpResponseMessage.Content.ReadAsStringAsync());
    }

    private async Task<ErrorOr<T>> SendHandleResponse<T>(HttpRequestMessage httpRequestMessage)
    {
        using var httpResponseMessage = await _client.SendAsync(httpRequestMessage);
        // if (_navigationManagerHandler.UnauthorizedHandler(httpResponseMessage.StatusCode))
        //     return Error.Custom(401, "Unauthorized", "Try to log in again");
        if (httpResponseMessage.StatusCode is not HttpStatusCode.OK)
            return Error.Failure(description: await httpResponseMessage.Content.ReadAsStringAsync());

        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var retVal = JsonConvert.DeserializeObject<T>(content);
        return retVal is not null
            ? retVal
            : Error.Failure(description: $"Failed to deserialize stream to {nameof(T)}");
    }
}