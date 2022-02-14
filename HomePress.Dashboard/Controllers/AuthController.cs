using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{

    public class AuthController : BaseController
    {
        public AuthController(DataService dataService) : base(dataService)
        {
        }

        [HttpGet("/auth/login")]
        public IActionResult Login()
        {
            if (IsAuthenticated())
                return Redirect("/dashboard");

            return View();
        }

        [HttpPost("/auth/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, bool cookie = true)
        {
            password = Core.Data.User.EncryptPassword(password);

            var user = await (await dataService.Users.FindAsync(f => f.Password == password && (f.Email == username || f.Phone == username || f.UserName == username))).FirstOrDefaultAsync();

            if (user == null)
            {
                ViewBag.Error = "Invalid credential";
                return View();
            }

            SetUserId(user.Id!, cookie);

            return Redirect("/dashboard");
        }

        [HttpGet("/auth/logout")]
        public IActionResult LogOut()
        {
            ClearUserId();

            return Redirect("/auth/login");
        }
    }
}
