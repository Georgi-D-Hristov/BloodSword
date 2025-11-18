using System;
using System.Collections.Generic;
using System.Text;

namespace BloodSword.Application.DTOs
{
    public class HeroInventoryDto
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public int Quantity { get; set; }
        public bool IsEquipped { get; set; }
    }
}
