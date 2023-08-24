using System.Reflection;
using System.Text.Json.Serialization;

using MangaHunter.API.Common.Mapping;
using MangaHunter.API.Errors;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

namespace MangaHunter.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddCors(options =>
            options.AddPolicy("defaultPolicy", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            ));
        services.AddSingleton<ProblemDetailsFactory, MangaHunterProblemDetailsFactory>();
        services.AddEndpointsApiExplorer();
        services.AddMappings();
        services.AddSwagger();
        // services.AddAuth0(configuration);
        services.AddHttpClient();

        return services;
    }

    private static void AddAuth0(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddAuthentication()
        //     .AddJwtBearer(options =>
        //     {
        //         options.Authority = $"https://{configuration["Auth0:Domain"]}";
        //         options.TokenValidationParameters =
        //             new TokenValidationParameters
        //             {
        //                 ValidAudience = configuration["Auth0:Audience"],
        //                 ValidIssuer = $"{configuration["Auth0:Domain"]}",
        //                 ValidateLifetime = true,
        //                 ClockSkew = TimeSpan.Zero
        //             };
        //     });
        // services.AddAuthorization();
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            // {
            //     Name = "Authorization",
            //     Type = SecuritySchemeType.ApiKey,
            //     Scheme = "Bearer",
            //     BearerFormat = "JWT",
            //     In = ParameterLocation.Header,
            //     Description =
            //         "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiX....\""
            // });
            // options.AddSecurityRequirement(new OpenApiSecurityRequirement
            // {
            //     {
            //         new OpenApiSecurityScheme
            //         {
            //             Reference = new OpenApiReference {Id = "Bearer", Type = ReferenceType.SecurityScheme}
            //         },
            //         Array.Empty<string>()
            //     }
            // });
            options.SwaggerDoc("v1", new OpenApiInfo {Title = "Manga Hunter", Version = "v1", Description = "",});
            options.CustomSchemaIds(type => type.ToString());

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
}