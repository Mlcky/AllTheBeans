using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Models;
using AllTheBeans.Core.Services;
using AllTheBeans.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Infrastructure.Services
{
    public class BeanOfTheDayService : IBeanOfTheDayService
    {
        private ApplicationDbContext _context;
        private readonly ICoffeeBeanService _coffeeBeanService;
        public BeanOfTheDayService(ApplicationDbContext context, ICoffeeBeanService coffeeBeanService)
        {
            _context = context;
            _coffeeBeanService = coffeeBeanService;
        }
        public async Task<CoffeeBeanDto> GetTodayAsync()
        {
            // get Todays date
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Get the bean in the beanofthedays table which has todays date.
            // Include the related fields, coffeebean, colour, country
            BeanOfTheDay? beanOfTheDay = await _context.BeanOfTheDays
                .Include(bean => bean.CoffeeBean)
                .ThenInclude(c => c.Colour)
                .Include(bean => bean.CoffeeBean)
                .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(b => b.Date == today);

            //If no bean today, create a new bean of the day
            if (beanOfTheDay is null)
            {
                // Create a new Bean Of the Day
                CoffeeBeanDto newCoffeeBeanDto = await this.GenerateNewAsync();
                return newCoffeeBeanDto;
            }
            // Return the bean of the day from the db
            return new CoffeeBeanDto
            {
                Id = beanOfTheDay.CoffeeBean.Id,
                Name = beanOfTheDay.CoffeeBean.Name,
                Description = beanOfTheDay.CoffeeBean.Description,
                Cost = beanOfTheDay.CoffeeBean.Cost,
                ImageUrl = beanOfTheDay.CoffeeBean.ImageUrl,
                Colour = beanOfTheDay.CoffeeBean.Colour.Name,
                Country = beanOfTheDay.CoffeeBean.Country.Name,
            };
        }
        public async Task<CoffeeBeanDto> GenerateNewAsync()
        {

            // Get yesterdays date
            var yestday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

            // Get the bean of the day from yesterday
            BeanOfTheDay? yesterdayBeanOfTheDay = await _context.BeanOfTheDays.FirstOrDefaultAsync(b => b.Date == yestday);


            // Get all the coffee beans
            List<CoffeeBean> coffeeBeans = await _context.CoffeeBeans
                .Include(coffeeBean => coffeeBean.Colour)
                .Include(coffeeBean => coffeeBean.Country)
                .ToListAsync();

            // If yesterday had a bean of the day, remove it from the list
            if (yesterdayBeanOfTheDay is not null)
            {
                coffeeBeans.RemoveAll(c => c.Id == yesterdayBeanOfTheDay.Id);
            }

            // If there are no coffee beans, return null
            if (coffeeBeans.Count == 0)
            {
                return null;
            }

            // Select a new botd at random
            Random rnd = new Random();
            int index = rnd.Next(0, coffeeBeans.Count);
            CoffeeBean beanOfTheDayCoffeeBean = coffeeBeans[index];

            if (beanOfTheDayCoffeeBean is null)
            {
                return null;
            }

            // use the bean selected to create a new bean of the day
            BeanOfTheDay beanOfTheDay = new BeanOfTheDay
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                CoffeeBean = beanOfTheDayCoffeeBean
            };

            // Try the db operations
            try
            {
                // Add the bean of the day to the db
                await _context.BeanOfTheDays.AddAsync(beanOfTheDay);
                await _context.SaveChangesAsync();

                return new CoffeeBeanDto
                {
                    Id = beanOfTheDayCoffeeBean.Id,
                    Name = beanOfTheDayCoffeeBean.Name,
                    Description = beanOfTheDayCoffeeBean.Description,
                    Cost = beanOfTheDayCoffeeBean.Cost,
                    ImageUrl = beanOfTheDayCoffeeBean.ImageUrl,
                    Colour = beanOfTheDayCoffeeBean.Colour.Name,
                    Country = beanOfTheDayCoffeeBean.Country.Name,
                };

            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
