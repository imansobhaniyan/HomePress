using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
