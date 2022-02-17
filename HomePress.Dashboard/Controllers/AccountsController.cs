using HomePress.Dashboard.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class AccountsController : BaseController
    {
        public AccountsController(DataService dataService) : base(dataService)
        {
        }

        #region Users

        [Route("/accounts/user")]
        public async Task<IActionResult> UserIndex(int pageNumber = 1, int pageSize = 20)
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            SetHeader("Users", "Users list", "Add new user", "/accounts/user/create");

            var query = dataService.Users.Find(f => f.UserName != "root" && f.Email != "root").SortByDescending(f => f.CreatedAt);

            var totalCount = await query.CountDocumentsAsync();

            var users = await query.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            return View(new BaseViewModel<User>(pageSize, pageNumber, totalCount, users));
        }

        [Route("/accounts/user/create")]
        [Route("/accounts/user/edit/{id}")]
        public async Task<IActionResult> UserForm(string id)
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            SetHeader("Users", (string.IsNullOrWhiteSpace(id) ? "New" : "Edit") + " user", "Add new user", "/accounts/user/create");

            var user = string.IsNullOrEmpty(id) ? new User() : await (await dataService.Users.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            if (user.UserName == "root" || user.Email == "root")
                return NotFound();

            return View("userform", user);
        }

        [HttpPost("/accounts/user/save")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSave([FromServices] IWebHostEnvironment environment, [FromForm] User model, [FromForm] string newPassword, IFormFile avatarFile)
        {
            try
            {
                var newAvaterPath = await SaveFileIfExistsAsync(environment, "avatars", avatarFile);

                if (!string.IsNullOrWhiteSpace(newAvaterPath))
                    model.AvatarUrl = newAvaterPath;

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword != "password")
                    model.Password = Core.Data.User.EncryptPassword(newPassword);

                model.UserName = model.Email;

                await dataService.SaveAsync(model);
            }
            catch
            {
                SetMessage(MessageType.FailSave);
            }

            return Redirect("/accounts/user");
        }

        [HttpPost("/accounts/user/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDelete(string ids)
        {
            try
            {
                await dataService.RemoveUsersAsync(ids.Split(','));

                SetMessage(MessageType.SuccessDelete);
            }
            catch
            {
                SetMessage(MessageType.FailDelete);
            }

            return Redirect("/accounts/user");
        }

        #endregion
    }
}
