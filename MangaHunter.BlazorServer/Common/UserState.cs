using System.Security.Claims;

using MudBlazor;

namespace MangaHunter.BlazorServer.Common;

public class UserState
{
    public UserState()
    {
        
    }

    public string Username { get; private set; } = "";
    public string UserId { get; set; } = "";
    public string UserIdFull { get; set; } = "";
    public string Picture { get; private set; } = "";
    public bool IsAuthenticated { get; private set; }
    public bool IsEmailVerified { get; private set; }

    public void LogIn(List<Claim> claims)
    {
        Username = claims
            .Where(c => c.Type.Equals("username"))
            .Select(c => c.Value)
            .FirstOrDefault() ?? string.Empty;

        UserIdFull = claims
            .Where(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
            .Select(c => c.Value).FirstOrDefault()!;
        
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

}