using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticCenter.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application layer services
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITestTypeService, TestTypeService>();
        services.AddScoped<ITestSetupService, TestSetupService>();
        services.AddScoped<ITestEntryService, TestEntryService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
