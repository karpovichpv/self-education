using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context, IWebHostEnvironment environment, UserManager<StoreUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("karpovich.pavel@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Pavel",
                    LastName = "Karpovich",
                    Email = "karpovich.pavel@gmail.com",
                    UserName = "karpovich.pavel@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Couldn't create new user in seeder");
                }
            }

            _context.Database.EnsureCreated();

            if (!_context.Products.Any())
            {
                string filePath = Path.Combine(_environment.ContentRootPath, "Data/art.json");
                // Need to create sample data
                var json = File.ReadAllText(filePath);
                IEnumerable<Product> products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

                _context.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price,
                        }
                    }
                };

                _context.Add(order);

                _context.SaveChanges();
            }
        }
    }
}
