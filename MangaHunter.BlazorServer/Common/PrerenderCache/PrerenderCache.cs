using System.Text.Json;

using Microsoft.JSInterop;

using Serilog;

namespace MangaHunter.BlazorServer.Common.PrerenderCache;

public interface IPrerenderCache
{
    Task<TResult> GetOrAdd<TResult>(string key, Func<Task<TResult>> factory);

    string Serialize();
}

public class PrerenderCache : IPrerenderCache
{
    private readonly IJSRuntime _js;

    public PrerenderCache(IJSRuntime js)
    {
        _js = js ?? throw new ArgumentNullException(nameof(js));
    }

    private Dictionary<string, object> Items { get; set; } = new();

    private bool _isLoaded;

    private bool IsRunningOnServer => _js.GetType().Name == "UnsupportedJavaScriptRuntime";

    public async Task<TResult> GetOrAdd<TResult>(string key, Func<Task<TResult>> factory)
    {
        if (IsRunningOnServer)
        {
            var res = await factory();

            if (res != null)
            {
                Items[key] = res;
            }

            return res;
        }

        try
        {
            await LoadFromJsAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return await factory();
        }

        if (!Items.Remove(key, out var item))
        {
            return await factory();
        }

        var json = JsonSerializer.Serialize(item);
        return JsonSerializer.Deserialize<TResult>(json)!;
    }

    private async Task LoadFromJsAsync()
    {
        if (!_isLoaded)
        {
            Items = await _js.InvokeAsync<Dictionary<string, object>>("prerenderCache.load");
            _isLoaded = true;
        }
    }

    public string Serialize() => JsonSerializer.Serialize(Items);
}