using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

using System.Data;

namespace HomePress.Core.Data
{
    public class DataService
    {
        private readonly TextSearchOptions searchOptions;

        private readonly IMongoCollection<Country> countries;
        private readonly IMongoCollection<State> states;
        private readonly IMongoCollection<City> cities;
        private readonly IMongoCollection<District> districts;

        private readonly IMongoCollection<User> users;

        private readonly IMongoCollection<Language> languages;

        private readonly IMongoCollection<Property> properties;


        public DataService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("default"));

            var db = client.GetDatabase("home_press");
            searchOptions = new TextSearchOptions { CaseSensitive = false, DiacriticSensitive = false };

            countries = db.GetCollection<Country>("countries");
            states = db.GetCollection<State>("states");
            cities = db.GetCollection<City>("cities");
            districts = db.GetCollection<District>("districts");
            initLocations().GetAwaiter().GetResult();

            users = db.GetCollection<User>("users");
            initRootUser().GetAwaiter().GetResult();

            languages = db.GetCollection<Language>("languages");

            properties = db.GetCollection<Property>("properties");

        }

        private async Task initRootUser()
        {
            if (!await (await Users.FindAsync(f => f.UserName == "root")).AnyAsync())
                await SaveAsync(new User
                {
                    UserName = "root",
                    FirstName = "root",
                    LastName = "user",
                    UserType = UserTypes.Admin,
                    UserStatus = UserStatus.Active,
                    Gender = Gender.Male,
                    LanguageIds = new List<string> { "en" },
                    Password = User.EncryptPassword("root")
                });
        }

        private async Task initLocations()
        {
            if (!await (await Countries.FindAsync(f => true)).AnyAsync())
            {
                var cont = await SaveAsync(new Country
                {
                    FullName = "Turkey",
                    Name = "Turkey",
                    PhoneCode = 90
                });

                var state = await SaveAsync(new State
                {
                    CountryId = cont.Id,
                    Name = "Antalya"
                });

                var city = await SaveAsync(new City
                {
                    CountryId = cont.Id,
                    StateId = state.Id,
                    Name = "Alanya"
                });

            }
        }

        #region Country

        public IMongoCollection<Country> Countries => countries;

        public async Task<Country> SaveAsync(Country item)
        {
            var update = !string.IsNullOrEmpty(item.Id);

            item.ModifiedAt = DateTime.UtcNow;

            if (update)
                await countries.ReplaceOneAsync(f => f.Id == item.Id, item);
            else
            {
                item.CreatedAt = DateTime.UtcNow;
                await countries.InsertOneAsync(item);
            }
            return item;
        }

        #endregion

        #region States

        public IMongoCollection<State> States => states;

        public async Task<State> SaveAsync(State item)
        {
            var update = !string.IsNullOrEmpty(item.Id);

            item.ModifiedAt = DateTime.UtcNow;

            if (update)
                await states.ReplaceOneAsync(f => f.Id == item.Id, item);
            else
            {
                item.CreatedAt = DateTime.UtcNow;
                await states.InsertOneAsync(item);
            }
            return item;
        }

        #endregion

        #region Cities

        public IMongoCollection<City> Cities => cities;

        public async Task<City> SaveAsync(City item)
        {
            var update = !string.IsNullOrEmpty(item.Id);

            item.ModifiedAt = DateTime.UtcNow;

            if (update)
                await cities.ReplaceOneAsync(f => f.Id == item.Id, item);
            else
            {
                item.CreatedAt = DateTime.UtcNow;
                await cities.InsertOneAsync(item);
            }
            return item;
        }

        #endregion

        #region Districts

        public IMongoCollection<District> Districts => districts;

        public async Task<District> SaveAsync(District item)
        {
            var update = !string.IsNullOrEmpty(item.Id);

            item.ModifiedAt = DateTime.UtcNow;

            if (update)
                await districts.ReplaceOneAsync(f => f.Id == item.Id, item);
            else
            {
                item.CreatedAt = DateTime.UtcNow;
                await districts.InsertOneAsync(item);
            }

            return item;
        }

        public async Task RemoveDistrictsAsync(params string[] itemIds)
        {
            await districts.DeleteManyAsync(new FilterDefinitionBuilder<District>().In(f => f.Id, itemIds));
        }

        #endregion

        #region Users

        public IMongoCollection<User> Users => users;

        public async Task<User> SaveAsync(User item)
        {
            var update = !string.IsNullOrEmpty(item.Id);

            item.ModifiedAt = DateTime.UtcNow;

            if (update)
                await users.ReplaceOneAsync(f => f.Id == item.Id, item);
            else
            {
                item.CreatedAt = DateTime.UtcNow;
                await users.InsertOneAsync(item);
            }

            return item;
        }

        public async Task RemoveUsersAsync(params string[] itemIds)
        {
            await users.DeleteManyAsync(new FilterDefinitionBuilder<User>().In(f => f.Id, itemIds));
        }

        #endregion

        #region Languages

        public IMongoCollection<Language> Languages => languages;

        public async Task<Language> SaveAsync(Language item)
        {
            var update = !string.IsNullOrEmpty(item.Id);

            item.ModifiedAt = DateTime.UtcNow;

            if (update)
                await languages.ReplaceOneAsync(f => f.Id == item.Id, item);
            else
            {
                item.CreatedAt = DateTime.UtcNow;
                await languages.InsertOneAsync(item);
            }
            return item;
        }

        public async Task RemoveLanguagesAsync(params string[] itemIds)
        {
            await languages.DeleteManyAsync(new FilterDefinitionBuilder<Language>().In(f => f.Id, itemIds));
        }

        #endregion

        #region Properties

        public IMongoCollection<Property> Properties => properties;

        public async Task<Property> SaveAsync(Property item)
        {
            var update = !string.IsNullOrEmpty(item.Id);
            item.ModifiedAt = DateTime.UtcNow;
            if (update)
            {
                var p = await (await properties.FindAsync(f => f.Id == item.Id)).FirstOrDefaultAsync();
                item.PropertyID = p.PropertyID;
                item.CreatedAt = p.CreatedAt;
                if (item.Photos == null)
                    item.Photos = p.Photos;
                if (item.Tags == null)
                    item.Tags = p.Tags;
                if (item.UnitFacades == null)
                    item.UnitFacades = p.UnitFacades;
                if (string.IsNullOrWhiteSpace(item.CreatorUserId))
                    item.CreatorUserId = p.CreatorUserId;
                await properties.ReplaceOneAsync(f => f.Id == item.Id, item);
            }
            else
            {
                var lastId = await (await properties.FindAsync(f => true)).AnyAsync() ? await Properties.Find(f => true).SortByDescending(f => f.PropertyID).Project(f => f.PropertyID).FirstOrDefaultAsync() : 10010;
                item.PropertyID = lastId + 1;
                item.CreatedAt = DateTime.UtcNow;
                item.PropertyStatus = PropertyStatus.Draft;
                await properties.InsertOneAsync(item);
            }
            return item;
        }

        public async Task RemovePropertiesAsync(params string[] itemIds)
        {
            await properties.DeleteManyAsync(new FilterDefinitionBuilder<Property>().In(f => f.Id, itemIds));
        }

        #endregion
    }
}
