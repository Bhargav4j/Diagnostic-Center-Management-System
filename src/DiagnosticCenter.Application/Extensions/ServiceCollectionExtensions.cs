using Microsoft.Extensions.DependencyInjection;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Application.Interfaces;

namespace DiagnosticCenter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        services.AddScoped<ITestTypeService, TestTypeService>();
        services.AddScoped<ITestSetupService, TestSetupService>();
        services.AddScoped<ITestEntryService, TestEntryService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
