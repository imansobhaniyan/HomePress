namespace HomePress.Dashboard.ViewModels
{
    public class DistrictVewModel : BaseViewModel<District>
    {
        public DistrictVewModel(int pageSize, int pageNumber, long totalItemsCount, List<District> items, List<Country> countries, List<State> states, List<City> cities) 
            : base(pageSize, pageNumber, totalItemsCount, items)
        {
            Countries = countries;
            States = states;
            Cities = cities;
        }

        public List<Country> Countries { get; set; }

        public List<State> States { get; set; }

        public List<City> Cities { get; set; }
    }
}
