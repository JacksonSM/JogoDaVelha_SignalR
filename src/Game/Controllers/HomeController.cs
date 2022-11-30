using Microsoft.AspNetCore.Mvc;

namespace Game.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
