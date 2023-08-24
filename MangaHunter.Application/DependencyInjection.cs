using System.Reflection;

using FluentValidation;

using MangaHunter.Application.Common.Behaviors;
using MangaHunter.Application.Common.Mapping;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace MangaHunter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>),typeof(ValidationBehaviors<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMappings();
        return services;
    }
}