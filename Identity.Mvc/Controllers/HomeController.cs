using Microsoft.AspNetCore.Mvc;

namespace Identity.Mvc.Controllers;

public class HomeController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}
