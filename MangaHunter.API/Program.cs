using CardboardBox.Http;
using CardboardBox.Json;

using MangaDexSharp;

using MangaHunter.API;
using MangaHunter.Application;
using MangaHunter.Infrastructure;
using MangaHunter.Infrastructure.Persistence;

using Microsoft.AspNetCore.HttpOverrides;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
    .MinimumLevel.Override("System", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Literate)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    {
        builder.Host.UseSerilog();
        builder.Services
            .AddPresentation(builder.Configuration)
            .AddApplication()
            .AddInfrastructures(builder.Configuration, builder.Environment);
        builder.Services
            .AddMangaDex()
            .AddJson()
            .AddCardboardHttp();
    }

    var app = builder.Build();
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        app.UseHsts();
        app.UseForwardedHeaders(new ForwardedHeadersOptions()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        // if (app.Environment.IsDevelopment())
        // {
        // }
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseExceptionHandler("/error");

        //app.UseHttpsRedirection();
        //app.UseAuthentication();
        //app.UseAuthorization();

        app.UseCors();
        app.MapControllers();
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