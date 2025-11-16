using BloodSword.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodSword.Application.DTOs
{
    public class CreateHeroDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(HeroClass))] 
        public HeroClass Class { get; set; }
    }
}