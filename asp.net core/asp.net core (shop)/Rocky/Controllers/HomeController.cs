using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System.Diagnostics;

namespace Rocky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public DetailsViewModel DetailsViewModel { get; private set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType),
                Categories = _db.Category,
            };

            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = [];
            List<ShoppingCart> shoppingCarts = RockySessionExtensions.Get<List<ShoppingCart>>(HttpContext.Session, WebConstants.SessionCart);
            if (shoppingCarts != null && shoppingCarts.Count() > 0)
            {
                shoppingCartList = shoppingCarts;
            }

            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                    detailsViewModel.ExistsInCart = true;
            }

            return View(detailsViewModel);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartList = [];
            List<ShoppingCart> shoppingCarts = RockySessionExtensions.Get<List<ShoppingCart>>(HttpContext.Session, WebConstants.SessionCart);
            if (shoppingCarts != null && shoppingCarts.Count() > 0)
            {
                shoppingCartList = shoppingCarts;
            }

            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            RockySessionExtensions.Set(HttpContext.Session, WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            List<ShoppingCart> shoppingCarts = RockySessionExtensions.Get<List<ShoppingCart>>(HttpContext.Session, WebConstants.SessionCart);
            if (shoppingCarts != null && shoppingCarts.Count() > 0)
            {
                shoppingCartList = shoppingCarts;
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }

            RockySessionExtensions.Set(HttpContext.Session, WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
