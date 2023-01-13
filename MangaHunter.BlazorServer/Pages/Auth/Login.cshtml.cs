using Auth0.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MangaHunter.BlazorServer.Pages.Auth;

public class Login : PageModel
{
    public async Task OnGet(string returnUrl = "/")
    {
        var authProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();
        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authProperties);
    }
}