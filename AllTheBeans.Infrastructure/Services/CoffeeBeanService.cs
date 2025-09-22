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
    public class CoffeeBeanService : ICoffeeBeanService
    {
        private ApplicationDbContext _context;
        public CoffeeBeanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CoffeeBeanDto>> GetCoffeeBeansAsync()
        {

            // get all coffee beans including their related fields, colour and country
            List<CoffeeBeanDto> coffeeBeans = await _context.CoffeeBeans
                .Include(coffeeBean => coffeeBean.Colour)
                .Include(coffeeBean => coffeeBean.Country)
                .Select(coffeeBean => new CoffeeBeanDto
                {
                    Id = coffeeBean.Id,
                    Name = coffeeBean.Name,
                    Description = coffeeBean.Description,
                    Cost = coffeeBean.Cost,
                    ImageUrl = coffeeBean.ImageUrl,
                    Colour = coffeeBean.Colour.Name,
                    Country = coffeeBean.Country.Name
                })
                .ToListAsync();

            return coffeeBeans;
        }
        public async Task<CoffeeBeanDto?> GetByIdAsync(int id)
        {
            // Get a single coffee bean by its id, including its related fields colour and country
            CoffeeBeanDto? coffeeBean = await _context.CoffeeBeans
                .Include(coffeeBean => coffeeBean.Colour)
                .Include(coffeeBean => coffeeBean.Country)
                .Select(coffeeBean => new CoffeeBeanDto
                {
                    Id = coffeeBean.Id,
                    Name = coffeeBean.Name,
                    Description = coffeeBean.Description,
                    Cost = coffeeBean.Cost,
                    ImageUrl = coffeeBean.ImageUrl,
                    Colour = coffeeBean.Colour.Name,
                    Country = coffeeBean.Country.Name
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            return coffeeBean;
        }
        public async Task<CoffeeBeanDto> CreateAsync(CreateCoffeeBeanDto bean)
        {
            // Grab the country if it exists or create a new one
            Country? country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == bean.Country);
            if (country is null)
            {
                country = new Country
                {
                    Name = bean.Country
                };
            }

            // Grab the colour if it exists or create a new one
            Colour? colour = await _context.Colours.FirstOrDefaultAsync(c => c.Name == bean.Colour);
            if (colour is null)
            {
                colour = new Colour
                {
                    Name = bean.Colour
                };
            }

            // Create a new coffee bean
            CoffeeBean coffeeBean = new CoffeeBean
            {
                Name = bean.Name,
                Description = bean.Description,
                Cost = bean.Cost,
                ImageUrl = bean.ImageUrl,
                Colour = colour,
                Country = country,
            };

            // save to the db
            try
            {
                await _context.AddAsync(coffeeBean);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            // create a dto to return to the user
            CoffeeBeanDto coffeeBeanDto = new CoffeeBeanDto
            {
                Id = coffeeBean.Id,
                Name = coffeeBean.Name,
                Description = coffeeBean.Description,
                Cost = coffeeBean.Cost,
                ImageUrl = coffeeBean.ImageUrl,
                Colour = coffeeBean.Colour.Name,
                Country = coffeeBean.Country.Name,
            };

            return coffeeBeanDto;
        }

        public async Task<CoffeeBeanDto> UpdateAsync(int id, UpdateCoffeeBeanDto bean)
        {
            // Get the existing coffee bean from the db
            CoffeeBean? coffeeBean = await _context.CoffeeBeans
                .Include(coffeeBean => coffeeBean.Colour)
                .Include(coffeeBean => coffeeBean.Country).FirstOrDefaultAsync(c => c.Id == id);

            if (coffeeBean is null)
            {
                return null;
            }

            // Grab the country if it exists or create a new one
            Country? country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == bean.Country);
            if (country is null)
            {
                country = new Country
                {
                    Name = bean.Country
                };
            }

            // Grab the colour if it exists or create a new one
            Colour? colour = await _context.Colours.FirstOrDefaultAsync(c => c.Name == bean.Colour);
            if (colour is null)
            {
                colour = new Colour
                {
                    Name = bean.Colour
                };
            }

            // Update the coffee bean
            coffeeBean.Name = bean.Name;
            coffeeBean.Description = bean.Description;
            coffeeBean.Cost = bean.Cost;
            coffeeBean.ImageUrl = bean.ImageUrl;
            coffeeBean.Colour = colour;
            coffeeBean.Country = country;

            // Save to the db
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            // Create a dto to return to the user
            CoffeeBeanDto coffeeBeanDto = new CoffeeBeanDto
            {
                Id = coffeeBean.Id,
                Name = coffeeBean.Name,
                Description = coffeeBean.Description,
                Cost = coffeeBean.Cost,
                ImageUrl = coffeeBean.ImageUrl,
                Colour = coffeeBean.Colour.Name,
                Country = coffeeBean.Country.Name
            };

            return coffeeBeanDto;
        }
        public async Task<bool> DeleteAsync(int id)
        {

            // get the existing coffee bean fromn the db
            CoffeeBean? beanToDelete = await _context.CoffeeBeans
                .Include(coffeeBean => coffeeBean.Colour)
                .Include(coffeeBean => coffeeBean.Country).FirstOrDefaultAsync(c => c.Id == id);

            // if it exists, delete it and save changes
            if (beanToDelete is not null)
            {
                _context.CoffeeBeans.Remove(beanToDelete);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
