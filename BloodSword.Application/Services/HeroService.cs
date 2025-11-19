using BloodSword.Application.Contracts;
using BloodSword.Application.DTOs;
using BloodSword.Domain.Entities;
using BloodSword.Domain.Enums;
using System;
using System.Threading.Tasks;

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
            // --- 1. Ръчно Мапване (DTO -> Entity) ---
            var hero = new Hero
            {
                Id = Guid.NewGuid(), // Генерираме ID тук
                Name = createHeroDto.Name,
                Class = createHeroDto.Class,
                Level = 1,
                Experience = 0
            };

            // --- 2. Прилагане на Бизнес Логика (Правилата на играта) ---
            // Това е мястото, където Service-ът блести!
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
            hero.CurrentEndurance = hero.Endurance; // Започва с пълна кръв

            // --- 3. Извикване на Репозиторито ---
            var newHero = await _heroRepository.CreateAsync(hero);

            // --- 4. Ръчно Мапване (Entity -> DTO) ---
            // Връщаме DTO, а не Entity-то!
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

            // Ръчно мапване на списък (малко досадно, но полезно упражнение)
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

        public async Task<HeroDto> GetHeroByIdAsync(Guid id)
        {
            var hero = await _heroRepository.GetByIdAsync(id);
            if (hero == null) return null; // Или хвърли Exception, ще го обсъдим

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
            // 1. Взимаме героя (с инвентара му, заради промяната в Repo-то)
            var hero = await _heroRepository.GetByIdAsync(heroId);
            if (hero == null) throw new Exception("Hero not found"); // По-добре ползвай къстъм Exception

            // 2. Взимаме предмета, за да сме сигурни, че съществува
            var item = await _itemRepository.GetByIdAsync(dto.ItemId);
            if (item == null) throw new Exception("Item not found");

            // 3. Проверяваме дали героят вече има този предмет (Business Logic)
            var existingInventoryItem = hero.Inventory.FirstOrDefault(ii => ii.ItemId == dto.ItemId);

            if (existingInventoryItem != null)
            {
                // Ако го има, просто увеличаваме бройката
                existingInventoryItem.Quantity += dto.Quantity;
            }
            else
            {
                // Ако го няма, добавяме нов запис в колекцията
                hero.Inventory.Add(new Domain.Entities.InventoryItem
                {
                    ItemId = item.Id,
                    HeroId = hero.Id,
                    Quantity = dto.Quantity
                });
            }

            // 4. Запазваме промените чрез Репозиторито на Героя
            await _heroRepository.UpdateAsync(hero);
        }

        public async Task EquipItemAsync(Guid heroId, Guid itemId)
        {
            var hero = await _heroRepository.GetByIdAsync(heroId);
            if (hero == null) throw new Exception("Hero not found");

            // Намираме предмета (купчината), който искаме да екипираме
            // Важно: Търсим такъв, който НЕ Е екипиран в момента
            var inventoryItemToEquip = hero.Inventory
                .FirstOrDefault(ii => ii.ItemId == itemId && !ii.IsEquipped);

            if (inventoryItemToEquip == null)
            {
                throw new Exception("Hero does not possess this item (or it is already equipped).");
            }

            // 1. Логика за сваляне на старите (Un-equip)
            var itemType = inventoryItemToEquip.Item.Type;
            var currentlyEquipped = hero.Inventory
                .Where(ii => ii.IsEquipped && ii.Item.Type == itemType)
                .ToList();

            foreach (var item in currentlyEquipped)
            {
                item.IsEquipped = false;

                // ТУК МОЖЕ ДА СЕ ДОБАВИ ЛОГИКА ЗА ОБЕДИНЯВАНЕ (MERGE) ОБРАТНО В СТАКА,
                // но за момента нека просто го свалим. Ще имаш два реда с Qty:1.
            }

            // 2. Логика за екипиране (С "Разцепване")
            if (inventoryItemToEquip.Quantity > 1)
            {
                // А: Намаляваме стака
                inventoryItemToEquip.Quantity -= 1;

                // Б: Създаваме нов запис за екипирания
                var newEquippedItem = new Domain.Entities.InventoryItem
                {
                    HeroId = hero.Id,
                    ItemId = itemId,
                    Quantity = 1,
                    IsEquipped = true
                };

                // Добавяме го към колекцията на героя
                hero.Inventory.Add(newEquippedItem);
            }
            else
            {
                // Ако е само 1 бройка, просто я екипираме
                inventoryItemToEquip.IsEquipped = true;
            }

            await _heroRepository.UpdateAsync(hero);
        }
    }
}