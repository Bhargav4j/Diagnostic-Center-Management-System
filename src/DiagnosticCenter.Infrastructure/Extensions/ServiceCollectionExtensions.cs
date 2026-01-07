using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;

namespace DiagnosticCenter.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DiagnosticCenterApp");

        services.AddDbContext<DiagnosticCenterDbContext>(options =>
            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(DiagnosticCenterDbContext).Assembly.FullName)));

        services.AddScoped<ITestTypeRepository, TestTypeRepository>();
        services.AddScoped<ITestSetupRepository, TestSetupRepository>();
        services.AddScoped<ITestEntryRepository, TestEntryRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
