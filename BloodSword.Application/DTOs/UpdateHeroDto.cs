using System.ComponentModel.DataAnnotations;

namespace BloodSword.Application.DTOs
{
    public class UpdateHeroDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        // Ние не позволяваме на клиента да променя Level или Stats директно,
        // защото това е работа на CombatService/LevelUpService.
    }
}
