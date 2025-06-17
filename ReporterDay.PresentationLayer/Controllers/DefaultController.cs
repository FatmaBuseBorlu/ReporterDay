using Microsoft.AspNetCore.Mvc;

namespace ReporterDay.PresentationLayer.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
