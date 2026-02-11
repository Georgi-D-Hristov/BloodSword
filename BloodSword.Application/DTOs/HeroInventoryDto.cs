namespace BloodSword.Application.DTOs
{
    public class HeroInventoryDto
    {
        public required string ItemName { get; set; }
        public required string ItemType { get; set; }
        public int Quantity { get; set; }
        public bool IsEquipped { get; set; }
    }
}
