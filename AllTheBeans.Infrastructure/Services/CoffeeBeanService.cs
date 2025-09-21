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
            return new CoffeeBeanDto();
        }

        public async Task<CoffeeBeanDto> UpdateAsync(int id, UpdateCoffeeBeanDto bean)
        {
            return new CoffeeBeanDto();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return true;
        }
    }
}
