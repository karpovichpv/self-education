﻿using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<Category> objList = _db.Category;
			return View(objList);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Category category)
		{
			_db.Category.Add(category);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
