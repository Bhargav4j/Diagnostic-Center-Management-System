using DiagnosticCenter.Application.Extensions;
using DiagnosticCenter.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting DiagnosticCenter.Web application");

    // Add services to the container.
    builder.Services.AddRazorPages();

    // Add session support
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(
            builder.Configuration.GetValue<int>("Session:IdleTimeout", 30));
        options.Cookie.Name = builder.Configuration["Session:CookieName"] ?? ".DiagnosticCenter.Session";
        options.Cookie.HttpOnly = builder.Configuration.GetValue<bool>("Session:CookieHttpOnly", true);
        options.Cookie.IsEssential = builder.Configuration.GetValue<bool>("Session:CookieIsEssential", true);
    });

    // Add authentication
    builder.Services.AddAuthentication("DiagnosticCenterAuth")
        .AddCookie("DiagnosticCenterAuth", options =>
        {
            options.LoginPath = builder.Configuration["Authentication:LoginPath"] ?? "/Index";
            options.AccessDeniedPath = builder.Configuration["Authentication:AccessDeniedPath"] ?? "/AccessDenied";
            options.Cookie.Name = builder.Configuration["Authentication:CookieName"] ?? ".DiagnosticCenter.Auth";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(
                builder.Configuration.GetValue<int>("Authentication:ExpireTimeSpan", 60));
            options.SlidingExpiration = true;
        });

    builder.Services.AddAuthorization();

    // Add HTTP context accessor
    builder.Services.AddHttpContextAccessor();

    // Add Infrastructure services (DbContext, Repositories)
    builder.Services.AddInfrastructure(builder.Configuration);

    // Add Application services
    builder.Services.AddApplicationServices();

    // Add distributed memory cache for session
    builder.Services.AddDistributedMemoryCache();

    var app = builder.Build();

    // Initialize database
    try
    {
        Log.Information("Initializing database...");
        await app.Services.InitializeDatabaseAsync(applyMigrations: true);
        Log.Information("Database initialized successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while initializing the database");
        // Don't stop the application, allow it to start even if database init fails
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSession();

    app.MapRazorPages();

    Log.Information("Application configured successfully, starting web host");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Make Program class accessible for integration tests
public partial class Program { }
