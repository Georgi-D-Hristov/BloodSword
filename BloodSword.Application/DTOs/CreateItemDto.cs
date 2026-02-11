using BloodSword.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodSword.Application.DTOs
{
    public class CreateItemDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public ItemType Type { get; set; }

        public int DamageModifier { get; set; }
        public int ArmorValue { get; set; }
    }
}