using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Models;
using AllTheBeans.Core.Services;
using AllTheBeans.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;


namespace AllTheBeans.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeanOfTheDayController : ControllerBase
    {
        private IBeanOfTheDayService _beanOfTheDayService;
        public BeanOfTheDayController(IBeanOfTheDayService beanOfTheDayService) {
            _beanOfTheDayService = beanOfTheDayService;
        }

        // Retreives the coffee bean of the day, if one is not already set, it sets a new bean of the day
        [HttpGet]
        public async Task<IActionResult> GetBeanOfTheDay()
        {
            CoffeeBeanDto? beanOfTheDay = await _beanOfTheDayService.GetTodayAsync();
            if (beanOfTheDay is null)
            {
                return NotFound();
            }
            return Ok(beanOfTheDay);
        }

    }
}
