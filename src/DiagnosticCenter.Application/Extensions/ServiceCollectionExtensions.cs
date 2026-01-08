using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticCenter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        services.AddScoped<ITestSetupService, TestSetupService>();
        services.AddScoped<ITestTypeService, TestTypeService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
