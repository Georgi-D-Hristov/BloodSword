using BloodSword.Domain.Enums;
using System;

namespace BloodSword.Application.DTOs
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Връщаме го като String за четимост
        public int DamageModifier { get; set; }
        public int ArmorValue { get; set; }
    }
}