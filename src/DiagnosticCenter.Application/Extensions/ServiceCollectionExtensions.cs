using DiagnosticCenter.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticCenter.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Register services
        services.AddScoped<ITestSetupService, TestSetupService>();
        services.AddScoped<ITestTypeService, TestTypeService>();
        services.AddScoped<ITestEntryService, TestEntryService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<DiagnosticCenter.Domain.Interfaces.Services.IAuthenticationService, AuthenticationService>();

        return services;
    }
}
