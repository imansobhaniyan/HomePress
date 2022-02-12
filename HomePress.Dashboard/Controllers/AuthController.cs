using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{

    public class AuthController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

    }

}
