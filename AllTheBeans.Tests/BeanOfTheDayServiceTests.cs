using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Models;
using AllTheBeans.Infrastructure;
using AllTheBeans.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace AllTheBeans.Tests
{
    public class BeanOfTheDayServiceTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }
        [Fact]
        public async Task GetTodayAsync_ShouldReturnTodaysBean_WhenExists()
        {
            using var context = GetDbContext(nameof(GetTodayAsync_ShouldReturnTodaysBean_WhenExists));

            var bean = new CoffeeBean
            {
                Name = "Ethiopian",
                Description = "Floral",
                Cost = 10m,
                ImageUrl = "http://ethiopia",
                Colour = new Colour { Name = "Light" },
                Country = new Country { Name = "Ethiopia" }
            };
            context.CoffeeBeans.Add(bean);
            await context.SaveChangesAsync();

            context.BeanOfTheDays.Add(new BeanOfTheDay
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                CoffeeBean = bean
            });
            await context.SaveChangesAsync();

            var service = new BeanOfTheDayService(context, new CoffeeBeanService(context));

            var result = await service.GetTodayAsync();

            result.Should().NotBeNull();
            result.Name.Should().Be("Ethiopian");
        }

        [Fact]
        public async Task GetTodayAsync_ShouldGenerateNew_WhenNoneExists()
        {
            using var context = GetDbContext(nameof(GetTodayAsync_ShouldGenerateNew_WhenNoneExists));

            var bean = new CoffeeBean
            {
                Name = "Colombian",
                Description = "Nutty",
                Cost = 12m,
                ImageUrl = "http://colombia",
                Colour = new Colour { Name = "Medium" },
                Country = new Country { Name = "Colombia" }
            };
            context.CoffeeBeans.Add(bean);
            await context.SaveChangesAsync();

            var service = new BeanOfTheDayService(context, new CoffeeBeanService(context));

            var result = await service.GetTodayAsync();

            result.Should().NotBeNull();
            result.Name.Should().Be("Colombian");

            // Should also be stored in DB
            var saved = await context.BeanOfTheDays.FirstOrDefaultAsync(b => b.Date == DateOnly.FromDateTime(DateTime.Now));
            saved.Should().NotBeNull();
        }

        [Fact]
        public async Task GenerateNewAsync_ShouldNotSelectYesterdayBean()
        {
            using var context = GetDbContext(nameof(GenerateNewAsync_ShouldNotSelectYesterdayBean));

            var bean1 = new CoffeeBean
            {
                Id = 1,
                Name = "Kenyan",
                Description = "Berry",
                Cost = 15m,
                ImageUrl = "http://kenya",
                Colour = new Colour { Name = "Dark" },
                Country = new Country { Name = "Kenya" }
            };
            var bean2 = new CoffeeBean
            {
                Id = 2,
                Name = "Guatemalan",
                Description = "Chocolate",
                Cost = 14m,
                ImageUrl = "http://guatemala",
                Colour = new Colour { Name = "Medium" },
                Country = new Country { Name = "Guatemala" }
            };

            context.CoffeeBeans.AddRange(bean1, bean2);
            await context.SaveChangesAsync();

            // Yesterday’s bean is bean1
            context.BeanOfTheDays.Add(new BeanOfTheDay
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                CoffeeBean = bean1
            });
            await context.SaveChangesAsync();

            var service = new BeanOfTheDayService(context, new CoffeeBeanService(context));

            var result = await service.GenerateNewAsync();

            result.Should().NotBeNull();
            result.Name.Should().Be("Guatemalan"); // only valid choice
        }

        [Fact]
        public async Task GenerateNewAsync_ShouldReturnNull_WhenNoBeansAvailable()
        {
            using var context = GetDbContext(nameof(GenerateNewAsync_ShouldReturnNull_WhenNoBeansAvailable));
            var service = new BeanOfTheDayService(context, new CoffeeBeanService(context));

            var result = await service.GenerateNewAsync();

            result.Should().BeNull();
        }



    }

}