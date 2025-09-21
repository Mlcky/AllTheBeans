using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AllTheBeans.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async void Seed(ApplicationDbContext context)
        {
            // check if there is already data in the db
            if (context.CoffeeBeans.Any())
            {
                return;
            }

            var rawJsonData = File.ReadAllText("AllTheBeans.Json");
            var beansFromJson = JsonSerializer.Deserialize<List<CoffeeBeanJsonDto>>(rawJsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if(beansFromJson is not null)
            {
                List<CoffeeBean> coffeeBeans = new List<CoffeeBean>();
                List<Colour> colours = new List<Colour>();
                List<Country> countries = new List<Country>();

                foreach (CoffeeBeanJsonDto coffeeBean in beansFromJson)
                {
                    // see if the country or color exists already
                    Country? country = countries.FirstOrDefault(c => c.Name == coffeeBean.Country);
                    Colour? colour = colours.FirstOrDefault(c => c.Name == coffeeBean.Colour);

                    if (country is null)
                    {
                        country = new Country { Name = coffeeBean.Country };
                        countries.Add(country);
                    }

                    if (colour is null)
                    {
                        colour = new Colour { Name = coffeeBean.Colour };
                        colours.Add(colour);
                    }

                    decimal cost;
                    bool convertDecimal = Decimal.TryParse(coffeeBean.Cost, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-GB"), out cost);

                    if (!convertDecimal)
                    {
                        Console.WriteLine($"Could not convert string {coffeeBean.Cost} to decimal");
                    }

                    coffeeBeans.Add(new CoffeeBean
                        {
                            Name = coffeeBean.Name,
                            Description = coffeeBean.Description,
                            Cost  = cost, // Need to change price to cost
                            ImageUrl = coffeeBean.ImageUrl,
                            Colour = colour,
                            Country = country

                        });
                }
                await context.AddRangeAsync(coffeeBeans);
                await context.SaveChangesAsync();
            }
            // got all data from json, need to loop through and push to db

        }
    }
}
