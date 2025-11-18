using BloodSword.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodSword.Application.DTOs
{
    public class CreateItemDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required]
        public ItemType Type { get; set; } // 1=Weapon, 2=Armor, etc.

        public int DamageModifier { get; set; }
        public int ArmorValue { get; set; }
    }
}