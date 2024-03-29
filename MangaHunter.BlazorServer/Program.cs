// using Auth0.AspNetCore.Authentication;

using MangaHunter.BlazorServer.Common;
using MangaHunter.BlazorServer.Common.PrerenderCache;
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
   
    const string scheme = "https";
    int port = builder.Environment.IsProduction() ? -1 : 7000;
    {
        builder.Host.UseSerilog();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMudServices();
        builder.Services.AddMemoryCache();

        // builder.Services
            // .AddAuth0WebAppAuthentication(options =>
            // {
            //     options.Domain = builder.Configuration["Auth0:Domain"]!;
            //     options.ClientId = builder.Configuration["Auth0:ClientId"]!;
            //     options.Scope = "openid profile email";
            //     options.OpenIdConnectEvents = new OpenIdConnectEvents
            //     {
            //         OnRedirectToIdentityProvider = context => OpenIdEvents.EnsureHttps(context, scheme, port),
            //         OnSignedOutCallbackRedirect = context => OpenIdEvents.EnsureHttps(context, scheme, port),
            //         OnRedirectToIdentityProviderForSignOut =
            //             context => OpenIdEvents.EnsureHttps(context, scheme, port),
            //     };
            // });
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
        builder.Services.AddScoped<IApiClient, ApiClient>();
        builder.Services.AddScoped<IApiService, ApiService>();
        builder.Services.AddScoped<IManagementApiService, ManagementApiService>();
        builder.Services.AddScoped<IPrerenderCache, PrerenderCache>();
        builder.Services.AddScoped<NavigationManagerHandler>();
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
            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next(context);
            });
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseCookiePolicy(options: new CookiePolicyOptions()
        {
            MinimumSameSitePolicy = SameSiteMode.None,
            Secure = CookieSecurePolicy.Always,
            HttpOnly = HttpOnlyPolicy.Always,
        });
        // app.UseAuthentication();
        // app.UseAuthorization();

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