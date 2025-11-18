using BloodSword.Domain.Enums;
using System;

namespace BloodSword.Application.DTOs
{
    public class HeroDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public HeroClass Class { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }

        // Обърни внимание, че не връщаме чувствителни данни
        // или сложни обекти като Inventory тук (засега).

        // Трябва ти и едно малко DTO за самия ред в инвентара
        public List<HeroInventoryDto> Inventory { get; set; } = new List<HeroInventoryDto>();
    }
}