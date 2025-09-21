using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.DTOs
{
    public class CoffeeBeanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Flattened navigation properties
        public string Country { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
    }
}
