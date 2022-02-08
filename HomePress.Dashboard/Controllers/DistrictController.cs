using HomePress.Dashboard.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class DistrictController : BaseController
    {
        public DistrictController(DataService dataService) : base(dataService)
        {
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20)
        {
            SetHeader("Districts", "Districts list", "Add new district", "/district/create");

            var query = dataService.Districts.Find(f => true).SortByDescending(f => f.CreatedAt);

            var totalCount = await query.CountDocumentsAsync();

            var districts = await query.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            var countries = await (await dataService.Countries.FindAsync(f => true)).ToListAsync();

            var states = await (await dataService.States.FindAsync(f => true)).ToListAsync();

            var cities = await (await dataService.Cities.FindAsync(f => true)).ToListAsync();

            return View(new DistrictVewModel(pageSize, pageNumber, totalCount, districts, countries, states, cities));
        }

        [Route("create")]
        [Route("edit/{id}")]
        public async Task<IActionResult> Form(string id)
        {
            SetHeader("District", (string.IsNullOrWhiteSpace(id) ? "New" : "Edit") + " district", "Add new district", "/district/create");

            var district = string.IsNullOrWhiteSpace(id) ? new District() : await (await dataService.Districts.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            return View("form", district);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromForm] District district)
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

            return Redirect("/district");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string ids)
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

            return Redirect("/district");
        }
    }
}
