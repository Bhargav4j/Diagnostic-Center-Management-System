using Microsoft.Extensions.DependencyInjection;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.Services;

namespace DiagnosticCenter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITestTypeService, TestTypeService>();

        return services;
    }
}
