using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Services;
using AllTheBeans.Core.Models;
using AllTheBeans.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;


namespace AllTheBeans.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoffeeBeanController : ControllerBase
    {
        private ICoffeeBeanService _coffeeBeanService;
        public CoffeeBeanController(ICoffeeBeanService coffeeBeanService)
        {
            _coffeeBeanService = coffeeBeanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoffeeBeans()
        {

            List<CoffeeBeanDto> coffeeBeans = await _coffeeBeanService.GetCoffeeBeansAsync();
            return Ok(coffeeBeans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoffeeBean(int id)
        {

            CoffeeBeanDto? coffeeBean = await _coffeeBeanService.GetByIdAsync(id);
            if (coffeeBean is null)
            {
                return NotFound();
            }
            return Ok(coffeeBean);
        }
    }
}
