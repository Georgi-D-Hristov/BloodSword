using BloodSword.Application.Contracts;
using BloodSword.Application.Services;
using BloodSword.Infrastructure.Persistence;
using BloodSword.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. РЕГИСТРАЦИЯ НА УСЛУГИ ---

// База данни
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Нашите Services и Repositories
builder.Services.AddScoped<IHeroRepository, HeroRepository>();
builder.Services.AddScoped<IHeroService, HeroService>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

// Контролери
builder.Services.AddControllers();

// SWAGGER (Важно!)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. MIDDLEWARE PIPELINE (Редът е важен!) ---

// Пускаме Swagger ВИНАГИ (махнахме if-а за теста)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Важно: Това кара контролерите да работят
app.MapControllers();

app.Run();