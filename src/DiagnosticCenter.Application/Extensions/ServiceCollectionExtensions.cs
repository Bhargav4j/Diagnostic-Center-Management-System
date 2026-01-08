using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.Mappings;
using DiagnosticCenter.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticCenter.Application.Extensions;

/// <summary>
/// Extension methods for configuring application services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application layer services to the service collection.
    /// Registers AutoMapper profiles and all application services.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Register application services
        services.AddScoped<ITestTypeService, TestTypeService>();
        services.AddScoped<ITestSetupService, TestSetupService>();
        services.AddScoped<ITestEntryService, TestEntryService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    /// <summary>
    /// Adds AutoMapper configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add AutoMapper to.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        return services;
    }

    /// <summary>
    /// Adds only the application services without AutoMapper to the service collection.
    /// Use this method if AutoMapper is registered separately.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddApplicationServicesOnly(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // Register application services
        services.AddScoped<ITestTypeService, TestTypeService>();
        services.AddScoped<ITestSetupService, TestSetupService>();
        services.AddScoped<ITestEntryService, TestEntryService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
