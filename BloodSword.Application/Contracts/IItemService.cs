using BloodSword.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodSword.Application.Contracts
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto> CreateItemAsync(CreateItemDto itemDto);
    }
}