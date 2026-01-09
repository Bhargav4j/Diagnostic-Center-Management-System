using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticCenter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITestTypeService, TestTypeService>();
        services.AddScoped<ITestSetupService, TestSetupService>();
        services.AddScoped<IPatientTestService, PatientTestService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
