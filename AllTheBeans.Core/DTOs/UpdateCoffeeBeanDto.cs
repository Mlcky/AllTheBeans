using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.DTOs
{
    public class UpdateCoffeeBeanDto
    {

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 9999.99)]
        public decimal Cost { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Colour { get; set; } = string.Empty;
    }
}
