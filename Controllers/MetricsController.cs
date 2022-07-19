using Microsoft.AspNetCore.Mvc;

namespace MVCLearning.Controllers
{
	public class MetricsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
