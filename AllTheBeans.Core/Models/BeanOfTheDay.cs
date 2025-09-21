using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.Models
{
    public class BeanOfTheDay
    {
        public int Id { get; set; }
        public int CoffeeBeanId { get; set; }
        public DateOnly Date { get; set; }
        public CoffeeBean? CoffeeBean { get; set; }

    }
}
