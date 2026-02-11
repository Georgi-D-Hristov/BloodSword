namespace BloodSword.Application.DTOs
{
    public class HeroInventoryDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool IsEquipped { get; set; }
    }
}
