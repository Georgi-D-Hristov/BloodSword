using BloodSword.Domain.Enums;

namespace BloodSword.Application.DTOs
{
    public class HeroDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public HeroClass Class { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public List<HeroInventoryDto> Inventory { get; set; } = new List<HeroInventoryDto>();
    }
}