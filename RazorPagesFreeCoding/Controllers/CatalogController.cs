using Microsoft.AspNetCore.Mvc;

using RazorPagesFreeCoding.Domain;
using RazorPagesFreeCoding.Infrastructure;

namespace RazorPagesFreeCoding.Controllers
{
	public class CatalogController : Controller
	{

		private readonly IMailSender _mailSender;

		private readonly Catalog _catalog;

		public CatalogController(Catalog catalog, IMailSender mailSender)
		{
			_catalog = catalog;
			_mailSender = mailSender;
		}

		[HttpGet]
		public IActionResult Products()
		{
			
			return View(_catalog);
		}

		[HttpGet]
		public IActionResult ProductsAdding()
		{
			return View();
		}


		[HttpPost]
		public IActionResult ProductsAdding([FromForm] string Name, decimal Price)
		{
			var product = new Product(Name, Price);
			_catalog.Add(product);
			Task.Run(() => _mailSender.SendMail());
			return View();

		}
	}
}
