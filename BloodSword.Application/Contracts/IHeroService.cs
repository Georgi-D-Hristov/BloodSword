using BloodSword.Application.DTOs;

namespace BloodSword.Application.Contracts
{
    public interface IHeroService
    {
        Task<HeroDto> CreateHeroAsync(CreateHeroDto createHeroDto);
        Task<IEnumerable<HeroDto>> GetAllHeroesAsync();
        Task<HeroDto?> GetHeroByIdAsync(Guid id);
        Task AddItemToHeroAsync(Guid heroId, AddHeroItemDto dto);
        Task EquipItemAsync(Guid heroId, Guid itemId);
        Task UpdateHeroAsync(Guid id, UpdateHeroDto dto);
        Task DeleteHeroAsync(Guid id);
    }
}
