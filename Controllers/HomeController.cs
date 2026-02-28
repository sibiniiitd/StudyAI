using Microsoft.AspNetCore.Mvc;

namespace StudyMonitor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
