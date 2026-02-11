using BloodSword.Application.Contracts;
using BloodSword.Application.DTOs;
using BloodSword.Domain.Entities;
using BloodSword.Domain.Enums;

namespace BloodSword.Application.Services
{
    public class HeroService : IHeroService
    {
        private readonly IHeroRepository _heroRepository;
        private readonly IItemRepository _itemRepository;

        public HeroService(IHeroRepository heroRepository, IItemRepository itemRepository)
        {
            _heroRepository = heroRepository;
            _itemRepository = itemRepository;
        }

        public async Task<HeroDto> CreateHeroAsync(CreateHeroDto createHeroDto)
        {
            var hero = new Hero
            {
                Id = Guid.NewGuid(),
                Name = createHeroDto.Name,
                Class = createHeroDto.Class,
                Level = 1,
                Experience = 0
            };

            // Apply game rules based on hero class
            switch (hero.Class)
            {
                case HeroClass.Warrior:
                    hero.FightingProwess = 10;
                    hero.PsychicAbility = 3;
                    hero.Awareness = 5;
                    hero.Endurance = 12;
                    break;
                case HeroClass.Sage:
                    hero.FightingProwess = 5;
                    hero.PsychicAbility = 10;
                    hero.Awareness = 8;
                    hero.Endurance = 8;
                    break;
                case HeroClass.Trickster:
                    hero.FightingProwess = 7;
                    hero.PsychicAbility = 5;
                    hero.Awareness = 10;
                    hero.Endurance = 9;
                    break;
                case HeroClass.Enchanter:
                    hero.FightingProwess = 4;
                    hero.PsychicAbility = 12;
                    hero.Awareness = 6;
                    hero.Endurance = 7;
                    break;
                default:
                    throw new ArgumentException("Invalid hero class selected.");
            }
            hero.CurrentEndurance = hero.Endurance;

            var newHero = await _heroRepository.CreateAsync(hero);

            var heroDto = new HeroDto
            {
                Id = newHero.Id,
                Name = newHero.Name,
                Class = newHero.Class,
                Level = newHero.Level,
                Experience = newHero.Experience
            };

            return heroDto;
        }

        public async Task<IEnumerable<HeroDto>> GetAllHeroesAsync()
        {
            var heroes = await _heroRepository.GetAllAsync();

            var heroDtos = new List<HeroDto>();
            foreach (var hero in heroes)
            {
                heroDtos.Add(new HeroDto
                {
                    Id = hero.Id,
                    Name = hero.Name,
                    Class = hero.Class,
                    Level = hero.Level,
                    Experience = hero.Experience
                });
            }
            return heroDtos;
        }

        public async Task<HeroDto?> GetHeroByIdAsync(Guid id)
        {
            var hero = await _heroRepository.GetByIdAsync(id);
            if (hero == null) return null;

            return new HeroDto
            {
                Id = hero.Id,
                Name = hero.Name,
                Class = hero.Class,
                Level = hero.Level,
                Experience = hero.Experience,
                Inventory = hero.Inventory.Select(i => new HeroInventoryDto
                {
                    ItemName = i.Item.Name,
                    ItemType = i.Item.Type.ToString(),
                    Quantity = i.Quantity,
                    IsEquipped = i.IsEquipped
                }).ToList()
            };
        }

        public async Task AddItemToHeroAsync(Guid heroId, AddHeroItemDto dto)
        {
            var hero = await _heroRepository.GetByIdAsync(heroId);
            if (hero == null) throw new Exception("Hero not found");

            var item = await _itemRepository.GetByIdAsync(dto.ItemId);
            if (item == null) throw new Exception("Item not found");

            var existingInventoryItem = hero.Inventory.FirstOrDefault(ii => ii.ItemId == dto.ItemId);

            if (existingInventoryItem != null)
            {
                existingInventoryItem.Quantity += dto.Quantity;
            }
            else
            {
                hero.Inventory.Add(new Domain.Entities.InventoryItem
                {
                    ItemId = item.Id,
                    HeroId = hero.Id,
                    Quantity = dto.Quantity
                });
            }

            await _heroRepository.UpdateAsync(hero);
        }

        public async Task EquipItemAsync(Guid heroId, Guid itemId)
        {
            var hero = await _heroRepository.GetByIdAsync(heroId);
            if (hero == null) throw new Exception("Hero not found");

            var inventoryItemToEquip = hero.Inventory
                .FirstOrDefault(ii => ii.ItemId == itemId && !ii.IsEquipped);

            if (inventoryItemToEquip == null)
            {
                throw new Exception("Hero does not possess this item (or it is already equipped).");
            }

            // Unequip items of the same type
            var itemType = inventoryItemToEquip.Item.Type;
            var currentlyEquipped = hero.Inventory
                .Where(ii => ii.IsEquipped && ii.Item.Type == itemType)
                .ToList();

            foreach (var item in currentlyEquipped)
            {
                item.IsEquipped = false;
            }

            // Equip the item (with stack splitting if necessary)
            if (inventoryItemToEquip.Quantity > 1)
            {
                inventoryItemToEquip.Quantity -= 1;

                var newEquippedItem = new Domain.Entities.InventoryItem
                {
                    HeroId = hero.Id,
                    ItemId = itemId,
                    Quantity = 1,
                    IsEquipped = true
                };

                hero.Inventory.Add(newEquippedItem);
            }
            else
            {
                inventoryItemToEquip.IsEquipped = true;
            }

            await _heroRepository.UpdateAsync(hero);
        }

        public async Task UpdateHeroAsync(Guid id, UpdateHeroDto dto)
        {
            var hero = await _heroRepository.GetByIdAsync(id);
            if (hero == null)
            {
                throw new KeyNotFoundException($"Hero with ID {id} not found.");
            }

            hero.Name = dto.Name;

            await _heroRepository.UpdateAsync(hero);
        }

        public async Task DeleteHeroAsync(Guid id)
        {
            await _heroRepository.DeleteAsync(id);
        }
    }
}