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

        // Retreives a list of all available coffee beans.
        [HttpGet]
        public async Task<IActionResult> GetCoffeeBeans()
        {

            List<CoffeeBeanDto> coffeeBeans = await _coffeeBeanService.GetCoffeeBeansAsync();
            return Ok(coffeeBeans);
        }

        // Retreives a specific cofee bean by its ID
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


        //Adds a new coffee bean to the collection
        [HttpPost]
        public async Task<IActionResult> AddCoffeeBean([FromBody] CreateCoffeeBeanDto bean)
        {
            try
            {
                CoffeeBeanDto coffeeBean = await _coffeeBeanService.CreateAsync(bean);
                return CreatedAtAction(nameof(GetCoffeeBean), new { id = coffeeBean.Id }, coffeeBean);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "An error occured while adding the coffee bean");
            }
        }

        // Updates an existing coffee bean identified by its ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoffeeBean(int id, [FromBody] UpdateCoffeeBeanDto bean)
        {
            try
            {
                CoffeeBeanDto coffeeBean = await _coffeeBeanService.UpdateAsync(id, bean);
                if (coffeeBean is null)
                {
                    return NotFound();
                }
                return Ok(coffeeBean);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "An error occured while updating the coffee bean");
            }
        }


        // Deletes a coffee bean identified by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoffeeBean(int id)
        {
            try
            {
                bool beanDeleted = await _coffeeBeanService.DeleteAsync(id);
                if (!beanDeleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "An error occured while adding the coffee bean");
            }
        }
    }
}
