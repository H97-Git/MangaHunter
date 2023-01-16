using System.Security.Claims;

using MudBlazor;

namespace MangaHunter.BlazorServer.Common;

public class UserState
{
    public UserState()
    {
        Theme = new MudTheme
        {
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = "12px",
                AppbarHeight = "60px",
            },
            Typography = new Typography
            {
                Default = new Default {FontFamily = new[] {"Montserrat"}, FontSize = "0.9rem",}
            },
            Palette = this.LightPalette,
            PaletteDark = this.DarkPalette,
        };
    }

    public string Username { get; private set; } = "";
    private string UserId { get; set; } = "";
    public string Picture { get; private set; } = "";
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

    public Palette LightPalette { get; } = new()
    {
        Primary = "#7e6fff",
        Secondary = "#503dff",
        Tertiary = "#494cfc",
        
        Surface = "#DEE2E6",
        Background = "#d6d6d6",
        BackgroundGrey = "#CED4DA", // MangaUpdateTable Scan Groups
        AppbarBackground = "#7e6fff",
        
        TextPrimary = "#212529",
        
        Info = "#4a86ff",
        Success = "#3dcb6c",
        Warning = "#ffb545",
        Error = "#ff3f5f",
        // Dark = null,

        LinesDefault = "#7e6fff",
        TableLines = "#33323e",
        Divider = "#6C757D",
    };

    public Palette DarkPalette { get; } = new()
    {
        Primary = "#7e6fff",
        Secondary = "#503dff",
        Tertiary = "#494cfc",
        
        Surface = "#343A40",
        Background = "#212529",
        BackgroundGrey = "#495057",
        AppbarBackground = "#343A40",
        
        TextPrimary = "#F8F9FA",
        TextSecondary = "#dcd8d8",
        
        Info = "#4a86ff",
        Success = "#3dcb6c",
        Warning = "#ffb545",
        Error = "#ff3f5f",
        // Dark = null,

        LinesDefault = "#7e6fff",
        TableLines = "#33323e",
        Divider = "#6C757D",
    };

    public MudTheme Theme { get; set; }

    public static ThemeManagerModel ThemeManager { get; set; } = new()
    {
        IsDarkMode = true, PrimaryColor = Colors.Indigo.Default
    };
}