using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DutchTreat.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<IEnumerable<Order>> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                IEnumerable<Order> result = _repository.GetAllOrders(includeItems);
                return base.Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest($"Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                Order order = _repository.GetOrderById(id);
                if (order != null)
                    return base.Ok(_mapper.Map<Order, OrderViewModel>(order));
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest($"Failed to get orders");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderViewModel order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Order newOrder = _mapper.Map<OrderViewModel, Order>(order);
                    if (newOrder.OrderDate == DateTime.MinValue)
                        newOrder.OrderDate = DateTime.Now;

                    _repository.AddEntity(newOrder);
                    if (_repository.SaveAll())
                    {
                        OrderViewModel orderViewModel = _mapper.Map<Order, OrderViewModel>(newOrder);

                        return Created($"/api/orders/{orderViewModel.OrderId}", orderViewModel);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
            }

            return BadRequest($"Failed to save a new order");
        }
    }
}
