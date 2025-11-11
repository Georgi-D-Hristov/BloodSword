namespace BloodSword.Domain.Entities
{
    public class InventoryItem
    {
        // Primary Key за тази връзка (може и композитен, но Guid е по-лесно за начало)
        public Guid Id { get; set; }

        // Външни ключове (Foreign Keys)
        public Guid HeroId { get; set; }
        public Guid ItemId { get; set; }

        // Навигационни properties (за EF Core)
   
        public  Hero Hero { get; set; } = null!;
        public Item Item { get; set; } = null!;

        // Допълнителна информация (Payload)
        public int Quantity { get; set; } = 1;
        public bool IsEquipped { get; set; } = false;
    }
}