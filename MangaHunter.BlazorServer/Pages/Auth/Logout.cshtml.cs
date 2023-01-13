using Auth0.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MangaHunter.BlazorServer.Pages.Auth;

public class Logout : PageModel
{
    public async Task OnGet(string returnUrl = "/")
    {
        var authProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();
        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authProperties);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}