using HomePress.Dashboard.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class SettingsController : BaseController
    {
        public SettingsController(DataService dataService) : base(dataService)
        {
        }

        #region District

        [Route("/settings/district")]
        public async Task<IActionResult> DistrictIndex(int pageNumber = 1, int pageSize = 20)
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            SetHeader("Districts", "Districts list", "Add new district", "/settings/district/create");

            var query = dataService.Districts.Find(f => true).SortByDescending(f => f.CreatedAt);

            var totalCount = await query.CountDocumentsAsync();

            var districts = await query.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            var countries = await (await dataService.Countries.FindAsync(f => true)).ToListAsync();

            var states = await (await dataService.States.FindAsync(f => true)).ToListAsync();

            var cities = await (await dataService.Cities.FindAsync(f => true)).ToListAsync();

            return View(new DistrictVewModel(pageSize, pageNumber, totalCount, districts, countries, states, cities));
        }

        [Route("/settings/district/create")]
        [Route("/settings/district/edit/{id}")]
        public async Task<IActionResult> DistrictForm(string id)
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            SetHeader("District", (string.IsNullOrWhiteSpace(id) ? "New" : "Edit") + " district", "Add new district", "/settings/district/create");

            var district = string.IsNullOrWhiteSpace(id) ? new District() : await (await dataService.Districts.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            return View("districtform", district);
        }

        [HttpPost("/settings/district/save")]
        public async Task<IActionResult> DistrictSave([FromForm] District district)
        {
            try
            {
                await dataService.SaveAsync(district);

                SetMessage(MessageType.SuccessSave);
            }
            catch
            {
                SetMessage(MessageType.FailSave);
            }

            return Redirect("/settings/district");
        }

        [HttpPost("/settings/district/delete")]
        public async Task<IActionResult> DistrictDelete(string ids)
        {
            try
            {
                await dataService.RemoveDistrictsAsync(ids.Split(','));

                SetMessage(MessageType.SuccessDelete);
            }
            catch
            {
                SetMessage(MessageType.FailDelete);
            }

            return Redirect("/settings/district");
        }

        #endregion

        #region Language

        [Route("/settings/language")]
        public async Task<IActionResult> LanguageIndex(int pageNumber = 1, int pageSize = 20)
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            SetHeader("Languages", "Languages list", "Add new language", "/settings/language/create");

            var query = dataService.Languages.Find(f => true).SortByDescending(f => f.CreatedAt);

            var totalCount = await query.CountDocumentsAsync();

            var languages = await query.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            return View(new BaseViewModel<Language>(pageSize, pageNumber, totalCount, languages));
        }

        [Route("/settings/language/create")]
        [Route("/settings/language/edit/{id}")]
        public async Task<IActionResult> LanguageForm(string id)
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            SetHeader("Languages", (string.IsNullOrWhiteSpace(id) ? "New" : "Edit") + " language", "Add new language", "/settings/language/create");

            var language = string.IsNullOrWhiteSpace(id) ? new Language() : await (await dataService.Languages.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            return View("languageform", language);
        }

        [HttpPost("/settings/language/save")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LanguageSave([FromServices] IWebHostEnvironment environment, [FromForm] Language language, IFormFile flagFile)
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

            return Redirect("/settings/language");
        }

        [HttpPost("/settings/language/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LanguageDelete(string ids)
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

            return Redirect("/settings/language");
        }

        #endregion

    }
}
