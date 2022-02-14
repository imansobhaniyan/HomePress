using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DataService dataService) : base(dataService)
        {
        }

        [Route("/")]
        public IActionResult Index() => IsAuthenticated() ? Redirect("/dashboard") : Redirect("/auth/login");
    }
}
