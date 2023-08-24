using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Infrastructure.Persistence;
using MangaHunter.Infrastructure.Persistence.Interceptors;
using MangaHunter.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace MangaHunter.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructures(this IServiceCollection services,
        ConfigurationManager configuration, IHostEnvironment environment)
    {
        var isDevelopment = environment.IsDevelopment();
        var connectionString = configuration
            .GetConnectionString(isDevelopment ? "DataConnectionLocal" : "DataConnectionDocker");
        Log.Debug($"{connectionString}");
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>()!;
            if (isDevelopment)
            {
                options.UseSqlite(connectionString)
                    .AddInterceptors(auditableInterceptor);
            }
            else
            {
                options.UseSqlServer(connectionString)
                    .AddInterceptors(auditableInterceptor);
            }

            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped<MangadexService>();
        services.AddScoped<IMangadexService, CachedMangadexService>();

        services.AddScoped<MangaUpdatesService>();
        services.AddScoped<IMangaUpdatesService, CachedMangaUpdatesService>();

        services.AddStackExchangeRedisCache(options =>
        {
            string redisConnectionString =
                configuration.GetConnectionString(isDevelopment ? "RedisLocal" : "RedisDocker")!;
            options.Configuration = redisConnectionString;
        });

        services.AddScoped<HunterRepository>();
        services.AddScoped<TierListRepository>();
        services.AddScoped<IHunterRepository, CachedHunterRepository>();
        services.AddScoped<ITierListRepository, CachedTierListRepository>();
    }
}