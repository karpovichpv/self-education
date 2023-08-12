using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddEntity(object order)
        {
            _context.Add(order);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
                return _context
                        .Orders
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .ToList();

            return _context
                    .Orders
                    .ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Products were quered");
                return _context.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public Order GetOrderById(int id)
            => GetAllOrders(true)
            .Where(o => o.Id == id)
            .FirstOrDefault();

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _context.Products
                            .Where(p => p.Category == category)
                            .ToList();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
