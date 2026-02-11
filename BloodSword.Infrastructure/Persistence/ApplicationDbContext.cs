using BloodSword.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloodSword.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InventoryItem>()
                .HasKey(ii => ii.Id);

            builder.Entity<InventoryItem>()
                .HasOne(ii => ii.Hero)
                .WithMany(h => h.Inventory)
                .HasForeignKey(ii => ii.HeroId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InventoryItem>()
                .HasOne(ii => ii.Item)
                .WithMany()
                .HasForeignKey(ii => ii.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Item>()
                .HasIndex(i => i.Name)
                .IsUnique();
        }
    }
}