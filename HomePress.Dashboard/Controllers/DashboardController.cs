using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(DataService dataService) : base(dataService)
        {
        }

        public IActionResult Index()
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            return View();
        }
    }
}
