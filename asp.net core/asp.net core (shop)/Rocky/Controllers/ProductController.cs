using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;

namespace Rocky.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
		{
			_db = db;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> objList = _db.Product
				.Include(u => u.Category)
				.Include(u => u.ApplicationType);

			return View(objList);
		}

		public IActionResult Upsert(int? id)
		{
			ProductViewModel productViewModel = new()
			{
				Product = new Product(),
				CategorySelectList = _db.Category.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
				ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
			};

			if (id == null)
			{
				return View(productViewModel);
			}
			else
			{
				productViewModel.Product = _db.Product.Find(id);
				if (productViewModel.Product == null)
				{
					return NotFound();
				}

				return View(productViewModel);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductViewModel productViewModel)
		{

			if (ModelState.IsValid)
			{
				var files = HttpContext.Request.Form.Files;
				string webRootPath = _webHostEnvironment.WebRootPath;

				if (productViewModel.Product.Id == 0)
				{
					// Creating

					string upload = webRootPath + WebConstants.ImagePath;
					string fileName = Guid.NewGuid().ToString();
					string extension = Path.GetExtension(files[0].FileName);

					using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}

					productViewModel.Product.Image = fileName + extension;

					_db.Product.Add(productViewModel.Product);
				}
				else
				{
					var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productViewModel.Product.Id);

					if (files.Count > 0)
					{
						string upload = webRootPath + WebConstants.ImagePath;
						string fileName = Guid.NewGuid().ToString();
						string extension = Path.GetExtension(files[0].FileName);

						var oldFile = Path.Combine(upload, objFromDb.Image);

						if (System.IO.File.Exists(oldFile))
						{
							System.IO.File.Delete(oldFile);
						}

						using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
						{
							files[0].CopyTo(fileStream);
						}

						productViewModel.Product.Image = fileName + extension;
					}
					else
					{
						productViewModel.Product.Image = objFromDb.Image;
					}
					_db.Product.Update(productViewModel.Product);

				}

				productViewModel.CategorySelectList = _db.Category.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				});
				productViewModel.ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				});

				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(productViewModel);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
				return NotFound();

			Product product = _db.Product
				.Include(u => u.Category)
				.Include(u => u.ApplicationType)
				.FirstOrDefault(u => u.Id == id);

			if (product == null)
				return NotFound();

			return View(product);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{
			var obj = _db.Product.Find(id);

			if (obj == null)
			{
				return NotFound();
			}

			string upload = _webHostEnvironment.WebRootPath + WebConstants.ImagePath;
			string oldFile = Path.Combine(upload, obj.Image);

			if (System.IO.File.Exists(oldFile))
			{
				System.IO.File.Delete(oldFile);
			}

			_db.Product.Remove(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
