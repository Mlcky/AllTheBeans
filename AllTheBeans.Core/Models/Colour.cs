using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.Models
{
    public class Colour
    {
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<CoffeeBean> CoffeeBeans { get; set; } = new List<CoffeeBean>();
    }
}
