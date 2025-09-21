using Microsoft.EntityFrameworkCore;
using AllTheBeans.Core.Models;

namespace AllTheBeans.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<CoffeeBean> CoffeeBeans { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<BeanOfTheDay> BeanOfTheDays { get; set; }

    }
}
