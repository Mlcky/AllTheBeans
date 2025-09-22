using AllTheBeans.Core.Models;
using AllTheBeans.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.Services
{
    public interface IBeanOfTheDayService
    {
        Task<CoffeeBeanDto>GetTodayAsync();
        Task<CoffeeBeanDto>GenerateNewAsync();
    }
}
