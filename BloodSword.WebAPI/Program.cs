using BloodSword.Application.Contracts;
using BloodSword.Application.Services;
using BloodSword.Infrastructure.Persistence;
using BloodSword.Infrastructure.Repositories;
using BloodSword.WebAPI.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.File;

// Configure Serilog: Reads configuration from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting BloodSword WebAPI host...");

    var builder = WebApplication.CreateBuilder(args);

    // Replace the built-in .NET logger with Serilog
    builder.Host.UseSerilog();

    // 1. Service Registration
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Services/Repositories (DI)
    builder.Services.AddScoped<IHeroRepository, HeroRepository>();
    builder.Services.AddScoped<IHeroService, HeroService>();
    builder.Services.AddScoped<IItemRepository, ItemRepository>();
    builder.Services.AddScoped<IItemService, ItemService>();
    builder.Services.AddControllers();

    // Swagger (for Development)
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // 2. Middleware Pipeline
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        // Execute asynchronous seeding logic and wait for completion
        await BloodSword.Infrastructure.Persistence.ApplicationDbContextSeed.SeedRolesAndAdminAsync(services);
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}