using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System.Security.Claims;
using System.Text;

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender sender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = sender;
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
                ProductList = productList.ToList()
            };

            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() +
                "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            var subject = "New Inquery";
            string HtmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder productList = new StringBuilder();
            foreach (var prod in ProductUserViewModel.ProductList)
            {
                productList.Append($" - Name: {prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                ProductUserViewModel.ApplicationUser.FullName,
                ProductUserViewModel.ApplicationUser.Email,
                ProductUserViewModel.ApplicationUser.PhoneNumber,
                productList.ToString()
                );

            await _emailSender.SendEmailAsync(WebConstants.AdminEmail, subject, messageBody);

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View(ProductUserViewModel);
        }
    }
}
