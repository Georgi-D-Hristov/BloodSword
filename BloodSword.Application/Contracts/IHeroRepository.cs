using BloodSword.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace BloodSword.Application.Contracts
{
    public interface IHeroRepository
    {
        Task<Hero> GetByIdAsync(Guid id);
        Task<Hero> CreateAsync(Hero hero);
        Task UpdateAsync(Hero hero);
        Task DeleteAsync(Guid id);           
        Task<IEnumerable<Hero>> GetAllAsync();
        Task<bool> ExistsAsync(string name);
    }
}
                 