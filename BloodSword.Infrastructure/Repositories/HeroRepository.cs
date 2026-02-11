using BloodSword.Application.Contracts;
using BloodSword.Domain.Entities;
using BloodSword.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodSword.Infrastructure.Repositories
{
    public class HeroRepository : IHeroRepository
    {
        private readonly ApplicationDbContext _context;

        public HeroRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Hero> CreateAsync(Hero hero)
        {
            await _context.Heroes.AddAsync(hero);
            await _context.SaveChangesAsync();
            return hero;
        }

        public async Task DeleteAsync(Guid id)
        {
            var heroToDelete = await _context.Heroes.FindAsync(id);
            if (heroToDelete != null)
            {
                _context.Heroes.Remove(heroToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Hero?> GetByIdAsync(Guid id)
        {
            return await _context.Heroes
                .Include(h => h.Inventory)
                .ThenInclude(ii => ii.Item)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task UpdateAsync(Hero hero)
        {
            _context.Entry(hero).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Hero>> GetAllAsync()
        {
            return await _context.Heroes.ToListAsync();
        }
    }
}
