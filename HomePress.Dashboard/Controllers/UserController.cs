using HomePress.Dashboard.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class UserController : BaseController
    {
        public UserController(DataService dataService) : base(dataService)
        {
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20)
        {
            SetHeader("Users", "Users list", "Add new user", "/user/create");

            var query = dataService.Users.Find(f => f.Email != "root").SortByDescending(f => f.CreatedAt);

            var totalCount = await query.CountDocumentsAsync();

            var users = await query.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            return View(new BaseViewModel<User>(pageSize, pageNumber, totalCount, users));
        }

        [Route("create")]
        [Route("edit/{id}")]
        public async Task<IActionResult> Form(string id)
        {
            SetHeader("Users", (string.IsNullOrWhiteSpace(id) ? "New" : "Edit") + " user", "Add new user", "/user/create");

            var user = string.IsNullOrEmpty(id) ? new User() : await (await dataService.Users.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            return View("form", user);
        }

        [HttpPost("save")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromServices] IWebHostEnvironment environment, [FromForm] User model, [FromForm] string newPassword, IFormFile avatarFile)
        {
            try
            {
                var newAvaterPath = await SaveFileIfExistsAsync(environment, "avatars", avatarFile);

                if (!string.IsNullOrWhiteSpace(newAvaterPath))
                    model.AvatarUrl = newAvaterPath;

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword != "password")
                    model.Password = Core.Data.User.EncryptPassword(newPassword);

                await dataService.SaveAsync(model);
            }
            catch
            {
                SetMessage(MessageType.FailSave);
            }

            return Redirect("/user");
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string ids)
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

            return Redirect("/user");
        }
    }
}
