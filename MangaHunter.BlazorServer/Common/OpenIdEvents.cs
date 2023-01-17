using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace MangaHunter.BlazorServer.Common;

public static class OpenIdEvents
{
    public static Task EnsureHttps(RedirectContext context, string scheme, int port)
    {
        var uriBuilder = new UriBuilder(context.ProtocolMessage.RedirectUri) {Scheme = scheme, Port = port};
        context.ProtocolMessage.RedirectUri = uriBuilder.ToString();
        return Task.CompletedTask;
    }
    public static Task EnsureHttps(RemoteSignOutContext context, string scheme, int port)
    {
        if (context.ProtocolMessage?.RedirectUri == null)
        {
            return Task.CompletedTask;
        }
        var uriBuilder = new UriBuilder(context.ProtocolMessage.RedirectUri) {Scheme = scheme, Port = port};
        context.ProtocolMessage.RedirectUri = uriBuilder.ToString();
        return Task.CompletedTask;
    }
}