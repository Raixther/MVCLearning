
using Serilog;
using RazorPagesFreeCoding.Domain;
using RazorPagesFreeCoding.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
			return View(_catalog);
		}

		[HttpGet]
		public  IActionResult ProductsAdding()
		{
		 return View();
		}


		[HttpPost]
		public  IActionResult ProductsAdding([FromForm] Product product)
		{
			_catalog.Add(product);			
			return View();
		}
	}
}
