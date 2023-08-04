using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _environment;

        public DutchSeeder(DutchContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void Seed()
        {
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
