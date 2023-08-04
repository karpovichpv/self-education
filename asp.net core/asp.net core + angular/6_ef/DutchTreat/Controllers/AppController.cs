using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly DutchContext _context;

        public AppController(IMailService mailService, DutchContext context)
        {
            _mailService = mailService;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> results = _context.Products.ToList();
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send the Email
                _mailService.SendMessage("shawn@wildermuth.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent...";
                ModelState.Clear();
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Shop()
        {
            List<Product> results = _context.Products
                .OrderBy(prod => prod.Category)
                .ToList();

            return View(results);
        }
    }
}
