namespace BloodSword.Domain.Entities
{
    public class InventoryItem
    {
        public Guid Id { get; set; }
        public Guid HeroId { get; set; }
        public Guid ItemId { get; set; }

        public Hero Hero { get; set; } = null!;
        public Item Item { get; set; } = null!;

        public int Quantity { get; set; } = 1;
        public bool IsEquipped { get; set; } = false;
    }
}