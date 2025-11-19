using System;
using System.ComponentModel.DataAnnotations;

namespace BloodSword.Application.DTOs
{
    public class AddHeroItemDto
    {
        [Required]
        public Guid ItemId { get; set; }

        [Range(1, 99)]
        public int Quantity { get; set; } = 1;
    }
}