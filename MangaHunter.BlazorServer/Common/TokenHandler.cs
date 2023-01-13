using System.Net.Http.Headers;

using Microsoft.AspNetCore.Authentication;

namespace MangaHunter.BlazorServer.Common;

public class TokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public TokenHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_contextAccessor.HttpContext != null)
        {
            var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}