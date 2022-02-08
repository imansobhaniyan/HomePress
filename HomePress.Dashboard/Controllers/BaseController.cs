using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    [Route("{controller}")]
    public abstract class BaseController : Controller
    {
        protected readonly DataService dataService;

        protected BaseController(DataService dataService)
        {
            this.dataService = dataService;
        }

        protected void SetHeader(string title, string subTitle, string? addNewButtonTitle = null, string? addNewButtonLink = null)
        {
            ViewBag.Title = title;
            ViewBag.SubTitle = subTitle;
            ViewBag.AddButtonTitle = addNewButtonTitle;
            ViewBag.AddButtonLink = addNewButtonLink;
        }

        protected void SetMessage(MessageType messageType)
        {
        }

        protected async Task<string?> SaveFileIfExistsAsync([FromServices] IWebHostEnvironment environment, string directory, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var relativePath = Path.Combine("uploads", directory, DateTime.Now.ToFileTime().ToString(), file.FileName);

            var absolutePath = Path.Combine(environment.WebRootPath, relativePath);

            CreateDirectory(new FileInfo(absolutePath).Directory!);

            using (var stream = new FileStream(absolutePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return "/" + relativePath.Replace("\\", "/");

            static void CreateDirectory(DirectoryInfo directory)
            {
                if (!directory.Exists)
                {
                    if (directory.Parent != null)
                        CreateDirectory(directory.Parent);

                    directory.Create();
                }
            }
        }

        public enum MessageType
        {
            SuccessSave,
            FailSave,
            SuccessDelete,
            FailDelete
        }
    }
}
