using MongoDB.Driver;

namespace HomePress.Core.Data
{

    public class DataService
    {
        private IMongoDatabase db;
        private readonly TextSearchOptions searchOptions;

        private readonly IMongoCollection<Country> countries;
        private readonly IMongoCollection<State> states;
        private readonly IMongoCollection<City> cities;
        private readonly IMongoCollection<District> districts;


        private readonly IMongoCollection<Property> properties;


        public DataService(IMongoClient client, string dbName)
        {
            db = client.GetDatabase(dbName);
            searchOptions = new TextSearchOptions { CaseSensitive = false, DiacriticSensitive = false };

            countries = db.GetCollection<Country>("countries");
            states = db.GetCollection<State>("states");
            cities = db.GetCollection<City>("cities");
            districts = db.GetCollection<District>("districts");
            initLocations();

            properties = db.GetCollection<Property>("properties");

        }


        private void initLocations()
        {
            if (!Countries.Any())
            {
                var cont = Save(new Country
                {
                    FullName = "Turkey",
                    Name = "Turkey",
                    PhoneCode = 90
                }).Result;

                var state = Save(new State
                {
                    CountryId = cont.Id,
                    Name = "Antalya"
                }).Result;

                var city = Save(new City
                {
                    CountryId = cont.Id,
                    StateId = state.Id,
                    Name = "Alanya"
                }).Result;

            }
        }


        #region Country
        public IQueryable<Country> Countries
        {
            get
            {
                return countries.AsQueryable();
            }
        }
        public async Task<Country?> FindCountry(string id)
        {
            return await countries.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Country> Save(Country item)
        {
            var update = !string.IsNullOrEmpty(item.Id);
            if (update)
            {
                await countries.ReplaceOneAsync(f => f.Id == item.Id, item);
            }
            else
            {
                await countries.InsertOneAsync(item);
            }
            return item;
        }

        public async Task Remove(Country item)
        {
            if (string.IsNullOrEmpty(item.Id)) return;
            await countries.DeleteOneAsync(f => f.Id == item.Id);
        }
        #endregion

        #region States
        public IQueryable<State> States
        {
            get
            {
                return states.AsQueryable();
            }
        }
        public async Task<State?> FindState(string id)
        {
            return await states.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<State> Save(State item)
        {
            var update = !string.IsNullOrEmpty(item.Id);
            if (update)
            {
                await states.ReplaceOneAsync(f => f.Id == item.Id, item);
            }
            else
            {
                await states.InsertOneAsync(item);
            }
            return item;
        }

        public async Task Remove(State item)
        {
            if (string.IsNullOrEmpty(item.Id)) return;
            await states.DeleteOneAsync(f => f.Id == item.Id);
        }
        #endregion

        #region Cities
        public IQueryable<City> Cities
        {
            get
            {
                return cities.AsQueryable();
            }
        }
        public async Task<City?> FindCity(string id)
        {
            return await cities.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<City> Save(City item)
        {
            var update = !string.IsNullOrEmpty(item.Id);
            if (update)
            {
                await cities.ReplaceOneAsync(f => f.Id == item.Id, item);
            }
            else
            {
                await cities.InsertOneAsync(item);
            }
            return item;
        }

        public async Task Remove(City item)
        {
            if (string.IsNullOrEmpty(item.Id)) return;
            await cities.DeleteOneAsync(f => f.Id == item.Id);
        }
        #endregion

        #region Districts
        public IQueryable<District> Districts
        {
            get
            {
                return districts.AsQueryable();
            }
        }
        public async Task<District?> FindDistrict(string id)
        {
            return await districts.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<District> Save(District item)
        {
            var update = !string.IsNullOrEmpty(item.Id);
            if (update)
            {
                await districts.ReplaceOneAsync(f => f.Id == item.Id, item);
            }
            else
            {
                await districts.InsertOneAsync(item);
            }
            return item;
        }

        public async Task Remove(District item)
        {
            if (string.IsNullOrEmpty(item.Id)) return;
            await districts.DeleteOneAsync(f => f.Id == item.Id);
        }
        #endregion



        #region Properties
        public IQueryable<Property> Properties
        {
            get
            {
                return properties.AsQueryable();
            }
        }
        public async Task<Property?> FindProperty(string id)
        {
            return await properties.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Property> Save(Property item)
        {
            var update = !string.IsNullOrEmpty(item.Id);
            item.ModifiedAt = DateTime.UtcNow;
            if (update)
            {
                var p = await FindProperty(item.Id);
                item.PropertyID = p.PropertyID;
                item.CreatedAt = p.CreatedAt;
                await properties.ReplaceOneAsync(f => f.Id == item.Id, item);
            }
            else
            {
                var lastId = Properties.Any() ? Properties.Max(f => f.PropertyID) : 10010;
                item.PropertyID = lastId + 1;
                item.CreatedAt = DateTime.UtcNow;
                await properties.InsertOneAsync(item);
            }
            return item;
        }

        public async Task Remove(Property item)
        {
            if (string.IsNullOrEmpty(item.Id)) return;
            await properties.DeleteOneAsync(f => f.Id == item.Id);
        }
        #endregion




    }



}
