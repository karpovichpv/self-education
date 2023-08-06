using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly DutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(DutchRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _repository.GetAllProducts();
        }
    }
}
