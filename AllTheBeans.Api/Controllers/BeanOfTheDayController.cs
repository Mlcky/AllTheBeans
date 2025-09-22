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
