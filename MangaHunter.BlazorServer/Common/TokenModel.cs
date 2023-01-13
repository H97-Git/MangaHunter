
using System.Text.Json.Serialization;

namespace MangaHunter.BlazorServer.Common;

public class TokenModel
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; }
    [JsonPropertyName("token_type")] public string TokenType { get; set; }
    [JsonPropertyName("expires_in")] public int ExpireIn { get; set; } 
}