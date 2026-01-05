using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticCenter.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DiagnosticCenterDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DiagnosticCenterConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

        services.AddScoped<ITestTypeRepository, TestTypeRepository>();
        services.AddScoped<ITestSetupRepository, TestSetupRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
