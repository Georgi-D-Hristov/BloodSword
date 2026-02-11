using BloodSword.Domain.Enums;

namespace BloodSword.Domain.Entities
{
    /// <summary>
    /// Represents a game item such as weapons, armor, or consumables
    /// </summary>
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ItemType Type { get; set; }
        public int DamageModifier { get; set; }
        public int ArmorValue { get; set; }
    }
}