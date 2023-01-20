using System.Net;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace MangaHunter.BlazorServer.Common.Services;

public class NavigationManagerHandler
{
    public NavigationManagerHandler(NavigationManager navigationManager)
    {
        NavigationManager = navigationManager;
    }

    private NavigationManager NavigationManager { get; }
    private const string AuthenticationUri = "auth/login";
    private const string ReturnUrlParams = "returnUrl";

    public string ToBaseRelativePath()
    {
       return NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
    }
    public void NavigateTo(string s)
    {
        NavigationManager.NavigateTo(s);
    }

    public void Reload()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri);
    }
    
    public bool UnauthorizedHandler(HttpStatusCode statusCode)
    {
        if (statusCode is not HttpStatusCode.Unauthorized) return false;

        var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        var queryString = QueryHelpers.AddQueryString(AuthenticationUri, ReturnUrlParams, relativePath);
        NavigateTo(queryString);
        return true;
    }

    public void SearchHandler(string title, List<Guid> includedTags, List<Guid> excludedTags)
    {
        var query = new Dictionary<string, string?> {{"title", title}};
        if (includedTags.Count is not 0)
            query.Add("included", string.Join(",", includedTags));
        if (excludedTags.Count is not 0)
            query.Add("excluded", string.Join(",", excludedTags));
        NavigateTo(QueryHelpers.AddQueryString("search", query));
    }


    public void NavigateToPage(int page)
    {
        NavigateTo(NavigationManager.GetUriWithQueryParameter("p", page));
    }
}