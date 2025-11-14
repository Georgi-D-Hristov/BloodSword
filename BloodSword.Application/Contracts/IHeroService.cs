using BloodSword.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace BloodSword.Application.Contracts
{
    public interface IHeroService
    {
        // Ще приемаме DTO и ще връщаме DTO
        Task<HeroDto> CreateHeroAsync(CreateHeroDto createHeroDto);

        // По-късно ще добавим Get, Update, Delete...
    }
}
