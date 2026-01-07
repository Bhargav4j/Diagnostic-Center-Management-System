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
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<ITestTypeRepository, TestTypeRepository>();
        services.AddScoped<ITestSetupRepository, TestSetupRepository>();
        services.AddScoped<ITestEntryRepository, TestEntryRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
}
