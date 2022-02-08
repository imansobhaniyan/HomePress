using HomePress.Dashboard.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class LanguageController : BaseController
    {
        public LanguageController(DataService dataService) : base(dataService)
        {
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20)
        {
            SetHeader("Languages", "Languages list", "Add new language", "/language/create");

            var query = dataService.Languages.Find(f => true).SortByDescending(f => f.CreatedAt);

            var totalCount = await query.CountDocumentsAsync();

            var languages = await query.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            return View(new BaseViewModel<Language>(pageSize, pageNumber, totalCount, languages));
        }

        [Route("create")]
        [Route("edit/{id}")]
        public async Task<IActionResult> Form(string id)
        {
            SetHeader("Languages", (string.IsNullOrWhiteSpace(id) ? "New" : "Edit") + " language", "Add new language", "/language/create");

            var language = string.IsNullOrWhiteSpace(id) ? new Language() : await (await dataService.Languages.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            return View("form", language);
        }

        [HttpPost("save")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromServices] IWebHostEnvironment environment, [FromForm] Language language, IFormFile flagFile)
        {
            try
            {
                var newFlagPath = await SaveFileIfExistsAsync(environment, "flags", flagFile);

                if (!string.IsNullOrWhiteSpace(newFlagPath))
                    language.FlagUrl = newFlagPath;

                await dataService.SaveAsync(language);

                SetMessage(MessageType.SuccessSave);
            }
            catch
            {
                SetMessage(MessageType.FailSave);
            }

            return Redirect("/language");
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string ids)
        {
            try
            {
                await dataService.RemoveLanguagesAsync(ids.Split(','));

                SetMessage(MessageType.SuccessDelete);
            }
            catch
            {
                SetMessage(MessageType.FailDelete);
            }

            return Redirect("/language");
        }
    }
}
