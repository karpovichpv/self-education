﻿using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Controllers
{
    [ApiController]
    [Route("/api/orders/{orderId}/items")]
    public class OrderItemsController : Controller
    {
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IDutchRepository _repository;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger,
             IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = _repository.GetOrderById(orderId);
            if (order != null) return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            return NotFound("");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            Order order = _repository.GetOrderById(orderId);
            if (order != null)
            {
                OrderItem item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if (item != null)
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
            }
            return NotFound("");
        }
    }
}
