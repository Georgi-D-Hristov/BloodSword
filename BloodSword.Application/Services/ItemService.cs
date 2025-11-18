using BloodSword.Application.Contracts;
using BloodSword.Application.DTOs;
using BloodSword.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodSword.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }


        public async Task<ItemDto> CreateItemAsync(CreateItemDto itemDto)
        {
            if (await _itemRepository.ExistsAsync(itemDto.Name))
            {
                throw new InvalidOperationException($"Item with name '{itemDto.Name}' already exists.");
            }

            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Type = itemDto.Type,
                DamageModifier = itemDto.DamageModifier,
                ArmorValue = itemDto.ArmorValue
            };

            var createdItem = await _itemRepository.CreateAsync(item);

            return MapToDto(createdItem);
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(MapToDto);
        }

        // Helper метод за мапване (за да не повтаряме кода)
        private static ItemDto MapToDto(Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Type = item.Type.ToString(), // Enum към String ("Weapon")
                DamageModifier = item.DamageModifier,
                ArmorValue = item.ArmorValue
            };
        }
    }
}