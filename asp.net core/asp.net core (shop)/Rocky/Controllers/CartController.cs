using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System.Security.Claims;

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = [];

            List<ShoppingCart> existingShoppingCarts = RockySessionExtensions.Get<List<ShoppingCart>>(HttpContext.Session, WebConstants.SessionCart);
            if (existingShoppingCarts != null && existingShoppingCarts.Count() > 0)
            {
                // session exists
                shoppingCartList = existingShoppingCarts;
            }

            List<int> productsInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _db.Product.Where(u => productsInCart.Contains(u.Id));

            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = [];

            List<ShoppingCart> existingShoppingCarts = RockySessionExtensions.Get<List<ShoppingCart>>(HttpContext.Session, WebConstants.SessionCart);
            if (existingShoppingCarts != null && existingShoppingCarts.Count() > 0)
            {
                // session exists
                shoppingCartList = existingShoppingCarts;
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            RockySessionExtensions.Set<List<ShoppingCart>>(HttpContext.Session, WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = [];
            List<ShoppingCart> existingShoppingCarts = RockySessionExtensions.Get<List<ShoppingCart>>(HttpContext.Session, WebConstants.SessionCart);
            if (existingShoppingCarts != null && existingShoppingCarts.Count() > 0)
            {
                // session exists
                shoppingCartList = existingShoppingCarts;
            }

            List<int> productsInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _db.Product.Where(u => productsInCart.Contains(u.Id));

            ProductUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = productList
            };

            return View(ProductUserViewModel);
        }
    }
}
