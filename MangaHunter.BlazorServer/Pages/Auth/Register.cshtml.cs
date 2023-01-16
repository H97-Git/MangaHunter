using Auth0.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MangaHunter.BlazorServer.Pages.Auth;

public class Register : PageModel
{
    public async Task OnGet(string returnUrl = "/")
    {
        var authPropertiesSignUp = new LoginAuthenticationPropertiesBuilder()
            .WithParameter("screen_hint", "signup")
            .WithRedirectUri(returnUrl)
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authPropertiesSignUp);
    }
}