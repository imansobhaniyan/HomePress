using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/dashboard");
        }
    }
}
