using System.Security.Claims;

using MudBlazor;

namespace MangaHunter.BlazorServer.Common;

public class UserState
{
    public UserState()
    {
        Theme = new MudTheme
        {
            LayoutProperties = new LayoutProperties {AppbarHeight = "80px", DefaultBorderRadius = "12px"},
            Typography = new Typography
            {
                Default = new Default {FontFamily = new[] {"Montserrat"}, FontSize = "0.9rem",}
            },
            Palette = this.LightPalette,
            PaletteDark = this.DarkPalette,
        };
    }

    public string Username { get; private set; } = "";
    private string UserId { get; set; }
    public string Picture { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public bool IsEmailVerified { get; private set; }

    public void LogIn(List<Claim> claims)
    {
        Username = claims
            .Where(c => c.Type.Equals("username"))
            .Select(c => c.Value)
            .FirstOrDefault() ?? string.Empty;
        
        UserId = claims
            .Where(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
            .Select(c => c.Value).FirstOrDefault()
            !.Split("|")[1];
        
        Picture = claims.Where(c => c.Type.Equals("picture"))
            .Select(c => c.Value)
            .FirstOrDefault() ?? string.Empty;
        
        IsEmailVerified =
            claims.Where(c => c.Type.Equals("email_verified")).Select(c => c.Value).FirstOrDefault() is "true";

        IsAuthenticated = true;
    }

    public Palette LightPalette { get; set; } = new()
    {
        AppbarBackground = "#ff4646", TextPrimary = "#fff", Surface = "#E0E0E0"
    };

    public Palette DarkPalette { get; set; } = new()
    {
        Primary = "#7e6fff",
        Tertiary = "#7e6fff",
        Surface = "#1e1e2d",
        Background = "#1a1a27",
        BackgroundGrey = "#151521",
        AppbarText = "#92929f",
        AppbarBackground = "rgba(26,26,39,0.8)",
        DrawerBackground = "#1a1a27",
        ActionDefault = "#74718e",
        ActionDisabled = "#9999994d",
        ActionDisabledBackground = "#605f6d4d",
        TextPrimary = "#b2b0bf",
        TextSecondary = "#92929f",
        TextDisabled = "#ffffff33",
        DrawerIcon = "#92929f",
        DrawerText = "#92929f",
        GrayLight = "#2a2833",
        GrayLighter = "#1e1e2d",
        Info = "#4a86ff",
        Success = "#3dcb6c",
        Warning = "#ffb545",
        Error = "#ff3f5f",
        LinesDefault = "#33323e",
        TableLines = "#33323e",
        Divider = "#292838",
        OverlayLight = "#1e1e2d80"
    };

    public MudTheme Theme { get; set; }

    public static ThemeManagerModel ThemeManager { get; set; } = new()
    {
        IsDarkMode = true, PrimaryColor = Colors.Indigo.Default
    };
}