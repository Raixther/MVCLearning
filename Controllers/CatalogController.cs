using Microsoft.AspNetCore.Mvc;

using RazorPagesFreeCoding.Domain;

namespace RazorPagesFreeCoding.Controllers
{
	public class CatalogController : Controller
	{

		private readonly Catalog _catalog;

		public CatalogController(Catalog catalog)
		{
			_catalog = catalog;
		}

		[HttpGet]
		public IActionResult Products()
		{
			
			return View();
		}

		[HttpPost]
		public IActionResult ProductsAdding(Product product)
		{
			_catalog.Products.Add(product);
			return View();
		}

	}
}
