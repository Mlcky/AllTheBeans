using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.Models
{
    public class CoffeeBean
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public int CountryId { get; set; }

        public int ColourId { get; set; }

        [Range(0, 9999.99)]
        public decimal Cost { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Colour? Colour { get; set; }
        public Country? Country { get; set;}
    }
}
