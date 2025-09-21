using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.DTOs
{
    public class BeanOfTheDayDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public CoffeeBeanDto? CoffeeBean { get; set; }

    }
}
