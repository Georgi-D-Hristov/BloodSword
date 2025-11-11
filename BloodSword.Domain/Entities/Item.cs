using BloodSword.Domain.Enums;

namespace BloodSword.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ItemType Type { get; set; }

        // Статистики на предмета
        public int DamageModifier { get; set; } // За оръжия (напр. +1 damage)
        public int ArmorValue { get; set; }     // За брони (напр. 2 armor)
    }
}