using BloodSword.Domain.Entities;

namespace BloodSword.Application.Contracts
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(Guid id);
        Task<Item> CreateAsync(Item item);
        Task<bool> ExistsAsync(string name);
    }
}
