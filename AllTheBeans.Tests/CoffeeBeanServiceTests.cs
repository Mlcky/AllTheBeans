using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Models;
using AllTheBeans.Infrastructure;
using AllTheBeans.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace AllTheBeans.Tests
{
    public class CoffeeBeanServiceTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName) // unique name for isolation
                .Options;

            return new ApplicationDbContext(options);
        }
        
        [Fact]
        public async Task GetCoffeeBeansAsync_ShouldReturnAllBeans()
        {
            // Arrange
            using var context = GetDbContext(nameof(GetCoffeeBeansAsync_ShouldReturnAllBeans));
            var bean = new CoffeeBean
            {
                Name = "Ethiopian Yirgacheffe",
                Description = "Floral and citrus",
                Cost = 12.5m,
                ImageUrl = "http://image.com/ethiopia",
                Colour = new Colour { Name = "Light" },
                Country = new Country { Name = "Ethiopia" }
            };
            context.CoffeeBeans.Add(bean);
            await context.SaveChangesAsync();

            var service = new CoffeeBeanService(context);

            // Act
            var result = await service.GetCoffeeBeansAsync();

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Ethiopian Yirgacheffe");
            result[0].Colour.Should().Be("Light");
            result[0].Country.Should().Be("Ethiopia");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectBean()
        {
            using var context = GetDbContext(nameof(GetByIdAsync_ShouldReturnCorrectBean));
            var bean = new CoffeeBean
            {
                Name = "Colombian",
                Description = "Nutty",
                Cost = 10m,
                ImageUrl = "http://image.com/colombia",
                Colour = new Colour { Name = "Medium" },
                Country = new Country { Name = "Colombia" }
            };
            context.CoffeeBeans.Add(bean);
            await context.SaveChangesAsync();

            var service = new CoffeeBeanService(context);

            var result = await service.GetByIdAsync(bean.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Colombian");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenBeanDoesNotExist()
        {
            // Arrange
            using var context = GetDbContext(nameof(GetByIdAsync_ShouldReturnNull_WhenBeanDoesNotExist));
            var service = new CoffeeBeanService(context);
            var nonExistentId = 999;

            // Act
            var result = await service.GetByIdAsync(nonExistentId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewBean()
        {
            using var context = GetDbContext(nameof(CreateAsync_ShouldAddNewBean));
            var service = new CoffeeBeanService(context);

            var createDto = new CreateCoffeeBeanDto
            {
                Name = "Kenya AA",
                Description = "Berry and wine",
                Cost = 14.5m,
                ImageUrl = "http://image.com/kenya",
                Colour = "Dark",
                Country = "Kenya"
            };

            var result = await service.CreateAsync(createDto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Kenya AA");

            // Confirm persisted
            var beansInDb = await context.CoffeeBeans.ToListAsync();
            beansInDb.Should().HaveCount(1);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingBean()
        {
            using var context = GetDbContext(nameof(UpdateAsync_ShouldModifyExistingBean));
            var bean = new CoffeeBean
            {
                Name = "Old Name",
                Description = "Old desc",
                Cost = 9.99m,
                ImageUrl = "http://old",
                Colour = new Colour { Name = "Light" },
                Country = new Country { Name = "Brazil" }
            };
            context.CoffeeBeans.Add(bean);
            await context.SaveChangesAsync();

            var service = new CoffeeBeanService(context);

            var updateDto = new UpdateCoffeeBeanDto
            {
                Name = "New Name",
                Description = "New desc",
                Cost = 11.99m,
                ImageUrl = "http://new",
                Colour = "Dark",
                Country = "Ethiopia"
            };

            var result = await service.UpdateAsync(bean.Id, updateDto);

            result.Should().NotBeNull();
            result!.Name.Should().Be("New Name");

            var updatedBean = await context.CoffeeBeans.Include(c => c.Country).FirstAsync();
            updatedBean.Country.Name.Should().Be("Ethiopia");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBean_WhenExists()
        {
            using var context = GetDbContext(nameof(DeleteAsync_ShouldRemoveBean_WhenExists));
            var bean = new CoffeeBean
            {
                Name = "Delete Me",
                Description = "Temp",
                Cost = 5m,
                ImageUrl = "http://delete",
                Colour = new Colour { Name = "Medium" },
                Country = new Country { Name = "Mexico" }
            };
            context.CoffeeBeans.Add(bean);
            await context.SaveChangesAsync();

            var service = new CoffeeBeanService(context);

            var result = await service.DeleteAsync(bean.Id);

            result.Should().BeTrue();
            (await context.CoffeeBeans.CountAsync()).Should().Be(0);
        }
    }
}