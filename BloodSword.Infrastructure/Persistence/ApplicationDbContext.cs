using BloodSword.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloodSword.Infrastructure.Persistence
{
    // Наследяваме IdentityDbContext, за да получим наготово таблиците за потребители (AspNetUsers и т.н.)
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Нашите бизнес таблици
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ВАЖНО: Първо викаме базовия метод, за да се настроят Identity таблиците!
            base.OnModelCreating(builder);

            // Тук ще настройваме нашите специфични връзки (Fluent API)

            // Настройка на Many-to-Many връзката за Инвентара
            builder.Entity<InventoryItem>()
                .HasKey(ii => ii.Id); // Първичен ключ

            // Казваме: InventoryItem има един Hero, който има много InventoryItems
            builder.Entity<InventoryItem>()
                .HasOne(ii => ii.Hero)
                .WithMany(h => h.Inventory)
                .HasForeignKey(ii => ii.HeroId)
                .OnDelete(DeleteBehavior.Cascade); // Ако изтрием Героя, трием и инвентара му

            // Казваме: InventoryItem има един Item
            // (Тук не сме сложили обратна колекция в Item, което е ОК - еднопосочна връзка)
            builder.Entity<InventoryItem>()
                .HasOne(ii => ii.Item)
                .WithMany()
                .HasForeignKey(ii => ii.ItemId)
                .OnDelete(DeleteBehavior.Restrict); // Ако изтрием Предмет от номенклатурата, НЕ искаме автоматично да изчезне от инвентарите (по-безопасно е да гръмне грешка)
        }
    }
}