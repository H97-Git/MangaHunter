using System.Security.Claims;
using System.Text.Json;

using MudBlazor;

namespace MangaHunter.BlazorServer.Common;

public class UserState
{
    public string UserId { get; private set; } = "";
    public string Username { get; private set; } = "ecchi";
    public string Picture { get; private set; } = "";
    // public UserData? UserData { get; set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsAuthenticated { get; private set; } = true;

    public void MapFromClaims(List<Claim> claims)
    {
        UserId = claims
            .Where(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
            .Select(c => c.Value).First();
        Username = claims
            .Where(c => c.Type.Equals("https://mangahunter.org/claims/username"))
            .Select(c => c.Value)
            .First();
        Picture = claims.Where(c => c.Type.Equals("picture"))
            .Select(c => c.Value)
            .First();
        // UserData = JsonSerializer.Deserialize<UserData>(claims
        //     .Where(c => c.Type.Equals("https://mangahunter.org/claims/user_metadata"))
        //     .Select(c => c.Value)
        //     .First());
        IsEmailVerified =
            claims.Where(c => c.Type.Equals("email_verified")).Select(c => c.Value).FirstOrDefault() is "true";
        IsAuthenticated = true;
    }

    public MudTheme Theme { get; } = new()
    {
        LayoutProperties = new LayoutProperties {DefaultBorderRadius = "12px", AppbarHeight = "60px"},
        Typography = new Typography
        {
            Default = new Default {FontFamily = new[] {"Montserrat", "Ubuntu", "Roboto"}, FontSize = "0.9rem",}
        },
        Palette = Palettes.Latte
    };
}