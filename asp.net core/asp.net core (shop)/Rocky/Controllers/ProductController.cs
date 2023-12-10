using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _db;

		public ProductController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> objects = _db.Product;
			return View(objects);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Product obj)
		{
			_db.Product.Add(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
