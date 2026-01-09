using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticCenter.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DiagnosticCenterDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                    npgsqlOptions.MigrationsHistoryTable("__efmigrations_history", "public");
                })
            .UseSnakeCaseNamingConvention()
            .EnableSensitiveDataLogging(configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging")));

        services.AddScoped<ITestTypeRepository, TestTypeRepository>();
        services.AddScoped<ITestSetupRepository, TestSetupRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IPatientTestRepository, PatientTestRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
}
