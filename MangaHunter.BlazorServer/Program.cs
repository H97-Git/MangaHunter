using Auth0.AspNetCore.Authentication;

using MangaHunter.BlazorServer.Common.Services;
using MangaHunter.BlazorServer.Common.Services.HttpClients;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;

using MudBlazor.Services;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
    .MinimumLevel.Override("System", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Code)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    //string scheme = builder.Environment.IsProduction() ? "https" : "http";
    const string scheme = "https";
    int port = builder.Environment.IsProduction() ? -1 : 7000;
    {
        builder.Host.UseSerilog();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMudServices();
        builder.Services.AddMemoryCache();

        builder.Services
            .AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"]!;
                options.ClientId = builder.Configuration["Auth0:ClientId"]!;
                options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
                options.Scope = "openid profile email";
                options.OpenIdConnectEvents = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        var uriBuilder = new UriBuilder(context.ProtocolMessage.RedirectUri)
                        {
                            Scheme = scheme, Port = port
                        };
                        context.ProtocolMessage.RedirectUri = uriBuilder.ToString();
                        return Task.CompletedTask;
                    },
                    // OnRedirectToIdentityProviderForSignOut = context =>
                    // {
                    //     var uriBuilder = new UriBuilder(context.ProtocolMessage.RedirectUri)
                    //     {
                    //         Scheme = scheme, Port = port
                    //     };
                    //     context.ProtocolMessage.RedirectUri = uriBuilder.ToString();
                    //     return Task.CompletedTask;
                    // },
                };
            });
        // .WithAccessToken(options =>
        // {
        //     options.Audience = builder.Configuration["Auth0:Audience"];
        //     options.UseRefreshTokens = true;
        // });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
        builder.Services.AddHttpClient();
        // builder.Services.AddHttpContextAccessor();
        // builder.Services.AddScoped<TokenHandler>();
        // builder.Services.ConfigureAll<HttpClientFactoryOptions>(options =>
        // {
        //     options.HttpMessageHandlerBuilderActions.Add(httpMessageHandlerBuilder =>
        //     {
        //         httpMessageHandlerBuilder
        //             .AdditionalHandlers
        //             .Add(httpMessageHandlerBuilder.Services.GetRequiredService<TokenHandler>());
        //     });
        // });
        builder.Services.AddScoped<NavigationManagerHandler>();
        builder.Services.AddScoped<IApiService, ApiService>();
        builder.Services.AddScoped<IApiClient, ApiClient>();
    }

    var app = builder.Build();
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseCookiePolicy(options:new CookiePolicyOptions()
        {
            MinimumSameSitePolicy = SameSiteMode.None,
            Secure = CookieSecurePolicy.Always,
            HttpOnly = HttpOnlyPolicy.Always,
            OnAppendCookie = context =>
            {
                Log.Debug($"CookieName : {context.CookieName}");
                Log.Debug($"CookieOptions.SameSite : {context.CookieOptions.SameSite.ToString()}");
                Log.Debug($"CookieValue : {context.CookieValue}");
            } 
        });
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.Run();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}