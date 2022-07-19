using Microsoft.AspNetCore.Mvc;

using MVCLearning.Domain;

namespace MVCLearning.Controllers
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
		[HttpGet]
		public IActionResult ProductsAdding()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ProductsAdding( [FromForm] Product product)
		{

			await _catalog.Add(product);
			return View();
		}
	}
}
