using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    [Route("{controller}")]
    public class PropertiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("create")]
        [Route("edit/{id}")]
        public IActionResult PropertyForm()
        {
            return View();
        }
    }
}
