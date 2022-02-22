using HomePress.Dashboard.Utilities;
using HomePress.Dashboard.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace HomePress.Dashboard.Controllers
{
    public class PropertiesController : BaseController
    {
        public PropertiesController(DataService dataService) : base(dataService)
        {
        }

        [Route("/properties/{status?}")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20, string status = "mainlist")
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            var userId = GetUserId();

            SetHeader("Property", "Properties list", "Add new property", "/properties/create");

            var filterDefinitions = new List<FilterDefinition<Property>> { FilterDefinition<Property>.Empty };

            var mainListFilterDefinition = new FilterDefinitionBuilder<Property>().In(f => f.PropertyStatus, new List<PropertyStatus> { PropertyStatus.Published, PropertyStatus.Sold });
            var processingFilterDefinition = new FilterDefinitionBuilder<Property>().In(f => f.PropertyStatus, new List<PropertyStatus> { PropertyStatus.Media, PropertyStatus.Rejected, PropertyStatus.Review });
            var draftFilterDefinition = new FilterDefinitionBuilder<Property>().Where(f => f.PropertyStatus == PropertyStatus.Draft && f.CreatorUserId == userId);
            var archiveFilterDefinition = new FilterDefinitionBuilder<Property>().Where(f => f.PropertyStatus == PropertyStatus.Archived);

            ///
            /// Filtering goes here
            ///

            var allFilterDefinitionsExeptCurrentStatus = new FilterDefinitionBuilder<Property>().And(filterDefinitions);

            var mainListCountQuery = dataService.Properties.Find(new FilterDefinitionBuilder<Property>().And(allFilterDefinitionsExeptCurrentStatus, mainListFilterDefinition));
            var processingCountQuery = dataService.Properties.Find(new FilterDefinitionBuilder<Property>().And(allFilterDefinitionsExeptCurrentStatus, processingFilterDefinition));
            var draftCountQuery = dataService.Properties.Find(new FilterDefinitionBuilder<Property>().And(allFilterDefinitionsExeptCurrentStatus, draftFilterDefinition));
            var archiveCountQuery = dataService.Properties.Find(new FilterDefinitionBuilder<Property>().And(allFilterDefinitionsExeptCurrentStatus, archiveFilterDefinition));

            switch (status)
            {
                case "mainlist":
                    filterDefinitions.Add(mainListFilterDefinition);
                    break;
                case "processing":
                    filterDefinitions.Add(processingFilterDefinition);
                    break;
                case "draft":
                    filterDefinitions.Add(draftFilterDefinition);
                    break;
                case "archived":
                    filterDefinitions.Add(archiveFilterDefinition);
                    break;
            }

            var query = dataService.Properties.Find(new FilterDefinitionBuilder<Property>().And(filterDefinitions)).SortByDescending(f => f.CreatedAt);

            var totalCount = await query.CountDocumentsAsync();

            long
                mainListCount = await mainListCountQuery.CountDocumentsAsync(),
                processingCount = await processingCountQuery.CountDocumentsAsync(),
                draftCount = await draftCountQuery.CountDocumentsAsync(),
                archiveCount = await archiveCountQuery.CountDocumentsAsync();

            var properties = await query.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            var countries = await (await dataService.Countries.FindAsync(new FilterDefinitionBuilder<Country>().In(f => f.Id, properties.Select(f => f.CountryId).ToList()))).ToListAsync();
            var states = await (await dataService.States.FindAsync(new FilterDefinitionBuilder<State>().In(f => f.Id, properties.Select(f => f.StateId).ToList()))).ToListAsync();
            var cities = await (await dataService.Cities.FindAsync(new FilterDefinitionBuilder<City>().In(f => f.Id, properties.Select(f => f.CityId).ToList()))).ToListAsync();
            var users = await (await dataService.Users.FindAsync(new FilterDefinitionBuilder<User>().In(f => f.Id, properties.Select(f => f.CreatorUserId).ToList()))).ToListAsync();

            return View(new PropertyVewModel(pageSize, pageNumber, totalCount, properties, status)
                .SetCounts(mainListCount, processingCount, draftCount, archiveCount)
                .SetLists(countries, states, cities, users));
        }

        [Route("/properties/create")]
        [Route("/properties/edit/{id}")]
        public async Task<IActionResult> PropertyForm(string id)
        {
            if (!IsAuthenticated())
                return Redirect("/auth/login");

            SetHeader("Property", (string.IsNullOrWhiteSpace(id) ? "New" : "Edit") + " property", "Add new property", "/properties/create");

            var property = string.IsNullOrWhiteSpace(id) ? new Property() : await (await dataService.Properties.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            return View("propertyForm", property);
        }

        [HttpPost("/properties/save")]
        public async Task<IActionResult> PropertySave([FromForm] Property property, [FromForm] string mapLocation)
        {
            try
            {
                try
                {
                    double
                        lat = double.Parse(mapLocation.Split(',').FirstOrDefault()!.Trim()),
                        lng = double.Parse(mapLocation.Split(',').LastOrDefault()!.Trim());

                    property.MapLAT = lat.ToString();
                    property.MapLNG = lng.ToString();
                }
                catch { }

                if (string.IsNullOrWhiteSpace(property.Id))
                    property.CreatorUserId = GetUserId();

                await dataService.SaveAsync(property);

                SetMessage(MessageType.SuccessSave);
            }
            catch
            {
                SetMessage(MessageType.FailSave);
            }

            if (property.PropertyStatus == PropertyStatus.Draft)
                return Redirect("/properties/draft");

            return Redirect("/properties");
        }

        [HttpPost("/properties/delete")]
        public async Task<IActionResult> PropertyDelete(string ids)
        {
            try
            {
                await dataService.RemovePropertiesAsync(ids.Split(','));

                SetMessage(MessageType.SuccessDelete);
            }
            catch
            {
                SetMessage(MessageType.FailDelete);
            }

            var redirectUrl = Request.Headers["Referer"];

            if (redirectUrl == Microsoft.Extensions.Primitives.StringValues.Empty)
                redirectUrl = "/properties";

            return Redirect(redirectUrl);
        }

        [Route("/properties/media/{id}")]
        [Route("/properties/media/{page}/{id}")]
        public async Task<IActionResult> PropertyMedia(string id, string page)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            if (string.IsNullOrWhiteSpace(page))
                return Redirect($"/properties/media/photo/{id}");

            SetHeader("Property", "Properties " + page);

            var property = await (await dataService.Properties.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

            if (property == null)
                return NotFound();

            ViewBag.Page = page;

            return View(property);
        }

        [HttpPost("/properties/upload/{type}/{id}")]
        public async Task<IActionResult> PropertyUpload([FromServices] IWebHostEnvironment environment, string id, string type)
        {
            if (string.IsNullOrWhiteSpace(id) || Request.Form.Files.Count == 0)
                return NotFound();

            if (type == "photo")
                return await UploadPhoto(environment, id);

            throw new NotImplementedException();
        }

        [HttpPost("/properties/caption/{type}/{id}")]
        public async Task<IActionResult> PropertyCaption(string type, string id, string caption)
        {
            if (type == "photo")
            {
                var photo = await (await dataService.Photos.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

                photo.Caption = caption;

                await dataService.SaveAsync(photo);

                var property = await (await dataService.Properties.FindAsync(f => f.Id == photo.PropertyId)).FirstOrDefaultAsync();

                var propertyPhoto = property.Photos.FirstOrDefault(f => f.PhotoId == id);

                propertyPhoto.Caption = caption;

                await dataService.SaveAsync(property);

                return Ok();
            }

            throw new NotImplementedException();
        }

        [HttpPost("/properties/media/delete/{type}/{id}/{propertyId}")]
        public async Task<IActionResult> PropertyDeleteMedia(string type, string id, string propertyId)
        {
            if (type == "photo")
            {
                var property = await (await dataService.Properties.FindAsync(f => f.Id == propertyId)).FirstOrDefaultAsync();

                if (property != null)
                    property.Photos = property.Photos.Where(f => f.PhotoId != id).ToList();

                await dataService.SaveAsync(property);

                await dataService.RemovePhotosAsync(id);

                return Ok();
            }

            throw new NotImplementedException();
        }

        private async Task<IActionResult> UploadPhoto([FromServices] IWebHostEnvironment environment, string id)
        {
            var directory = Path.Combine("property", "photo");

            foreach (var file in Request.Form.Files)
            {
                var path = await SaveFileIfExistsAsync(environment, directory, file!);

                directory = Path.Combine(directory, DateTime.Now.ToFileTime().ToString());

                var photo = new Photo
                {
                    UserId = GetUserId(),
                    PropertyId = id,
                    OriginalUrl = path,
                    Width1920 = await file.SaveResizedAsync(environment, directory, 1920) ?? path,
                    Width1024 = await file.SaveResizedAsync(environment, directory, 1024) ?? path,
                    Width512 = await file.SaveResizedAsync(environment, directory, 512) ?? path,
                    Width256 = await file.SaveResizedAsync(environment, directory, 256) ?? path
                };

                await dataService.SaveAsync(photo);
                
                var property = await (await dataService.Properties.FindAsync(f => f.Id == id)).FirstOrDefaultAsync();

                property.Photos.Add(new PhotoCompact
                {
                    PhotoId = photo.Id,
                    Width1920 = photo.Width1920,
                    Width1024 = photo.Width1024,
                    Width512 = photo.Width512,
                    Width256 = photo.Width256
                });

                await dataService.SaveAsync(property);
            }

            return Ok();
        }
    }
}
