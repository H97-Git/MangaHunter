using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using ErrorOr;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using Newtonsoft.Json;

using Serilog;

namespace MangaHunter.BlazorServer.Common.Services;

public interface IManagementApiService
{
    public Task<ErrorOr<User>> UpdateUser(string userId, UserUpdateRequest userUpdateRequest);
    public Task<ErrorOr<User>> GetUser(string userId);
}

public class ManagementApiService : IManagementApiService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _tokenClient;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly string _domain;
    private string _audience;
    private string _tokenApi;
    private string _tokenKey = "Token";

    public ManagementApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        ProtectedLocalStorage protectedLocalStorage)
    {
        _configuration = configuration;
        _protectedLocalStorage = protectedLocalStorage;
        _tokenClient = httpClientFactory.CreateClient();
        _domain = _configuration.GetSection("Auth0").GetValue<string>("Domain")!;
        _audience = "https://" + _domain + _configuration.GetSection("Auth0").GetValue<string>("Audience");
        _tokenApi = "https://" + _domain + _configuration.GetSection("Auth0").GetValue<string>("TokenApi");
    }

    private async Task<ErrorOr<Auth0TokenResponse>> GetOrFetchToken()
    {
        var tokenFromLocalStorage = await _protectedLocalStorage.GetAsync<Auth0TokenResponse>(_tokenKey);
        if (tokenFromLocalStorage.Success && tokenFromLocalStorage.Value is not null)
        {
            var token = tokenFromLocalStorage.Value;

            DateTime now = DateTime.UtcNow;
            DateTime expiresAt = now.AddSeconds(token.expires_in);
            if (now < expiresAt)
            {
                return token;
            }
        }

        var newToken = await FetchToken();
        if (!newToken.IsError)
        {
            return newToken.Value;
        }

        return Error.Failure("Failed to get Token");
    }

    private async Task<ErrorOr<Auth0TokenResponse>> FetchToken()
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", _configuration.GetSection("Auth0").GetValue<string>("ClientId")!),
            new("client_secret", _configuration.GetSection("Auth0").GetValue<string>("ClientSecret")!),
            new("audience", _audience)
        };
        var req = new HttpRequestMessage(HttpMethod.Post, _tokenApi) {Content = new FormUrlEncodedContent(parameters)};
        var res = await _tokenClient.SendAsync(req);
        var response = await res.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<Auth0TokenResponse>(response);
        if (data == null)
        {
            return Error.Failure();
        }

        await _protectedLocalStorage.SetAsync(_tokenKey, data);
        return data;
    }

    private async Task<ErrorOr<ManagementApiClient>> GetClient()
    {
        var token = await GetOrFetchToken();
        if (token.IsError)
        {
            return token.Errors;
        }

        return new ManagementApiClient(token.Value.access_token, new Uri(_audience));
    }

    public async Task<ErrorOr<User>> UpdateUser(string userId, UserUpdateRequest userUpdateRequest)
    {
        var managementApiClient = await GetClient();
        if (managementApiClient.IsError)
        {
            return managementApiClient.Errors;
        }

        try
        {
            var response = await managementApiClient.Value.Users.UpdateAsync(userId, userUpdateRequest);
            return response;
        }
        catch (Exception e)
        {
            Log.Error("Failed to update user");
            return Error.Failure(e.Message);
        }
    }

    public async Task<ErrorOr<User>> GetUser(string userId)
    {
        var managementApiClient = await GetClient();
        if (managementApiClient.IsError)
        {
            return managementApiClient.Errors;
        }

        var response = await managementApiClient.Value.Users.GetAsync(userId);
        if (response is null)
            return Error.NotFound();
        return response;
    }
}

public class Auth0TokenResponse
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string scope { get; set; }
    public string token_type { get; set; }
}