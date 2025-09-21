using AllTheBeans.Core.Models;
using AllTheBeans.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.Services
{
    public interface ICoffeeBeanService
    {
        Task<List<CoffeeBeanDto>> GetCoffeeBeansAsync();
        Task<CoffeeBeanDto?> GetByIdAsync(int id);
        Task<CoffeeBeanDto> CreateAsync(CreateCoffeeBeanDto bean);
        Task<CoffeeBeanDto> UpdateAsync(int id, UpdateCoffeeBeanDto bean);
        Task<bool> DeleteAsync(int id);
    }
}
