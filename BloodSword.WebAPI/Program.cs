using BloodSword.Application.Contracts;
using BloodSword.Application.Services;
using BloodSword.Infrastructure.Persistence;
using BloodSword.Infrastructure.Repositories;
using BloodSword.WebAPI.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog; // <--- НОВ USING
using Serilog.Sinks.File; // <--- НОВ USING

// Настройка на Serilog: Чете конфигурацията от appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // <--- Запис във файл
    .CreateLogger();

try
{
    Log.Information("Starting BloodSword WebAPI host...");

    var builder = WebApplication.CreateBuilder(args);

    // Подменяме вградения логър на .NET с нашия Serilog
    builder.Host.UseSerilog();

    // 1. РЕГИСТРАЦИЯ НА УСЛУГИ (както преди)
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Services/Repositories (DI)
    builder.Services.AddScoped<IItemRepository, ItemRepository>();
    builder.Services.AddScoped<IHeroRepository, HeroRepository>();
    builder.Services.AddScoped<IHeroService, HeroService>();
    builder.Services.AddControllers();

    // SWAGGER (за Development)
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // 2. MIDDLEWARE PIPELINE
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly"); // Улавяме фатални грешки при стартиране
}
finally
{
    Log.CloseAndFlush();
}