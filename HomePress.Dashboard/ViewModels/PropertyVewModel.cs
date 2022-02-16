namespace HomePress.Dashboard.ViewModels
{
    public class PropertyVewModel : BaseViewModel<Property>
    {
        public PropertyVewModel(int pageSize, int pageNumber, long totalItemsCount, List<Property> items, string status)
            : base(pageSize, pageNumber, totalItemsCount, items)
        {
            Status = status;
        }

        public PropertyVewModel SetCounts(long mainListCount, long processingCount, long draftCount, long archiveCount)
        {
            MainListCount = mainListCount;
            ProcessingCount = processingCount;
            DraftCount = draftCount;
            ArchiveCount = archiveCount;

            return this;
        }

        public PropertyVewModel SetLists(List<Country> countries, List<State> states, List<City> cities, List<User> users)
        {
            Countries = countries;
            States = states;
            Cities = cities;
            Users = users;

            return this;
        }

        public string Status { get; set; }

        public long MainListCount { get; set; }

        public long ProcessingCount { get; set; }

        public long DraftCount { get; set; }

        public long ArchiveCount { get; set; }

        public List<Country> Countries { get; set; }

        public List<State> States { get; set; }

        public List<City> Cities { get; set; }

        public List<User> Users { get; set; }

        public string GetPretyCount(double count)
        {
            if (count == 0)
                return string.Empty;

            var character = "";

            if (count >= 1000)
            {
                count /= 1000;
                character = "K";
            }

            if (count >= 1000)
            {
                count /= 1000;
                character = "M";
            }

            if (string.IsNullOrEmpty(character))
                return $"({Math.Round(count, 2)})";

            return $"({Math.Round(count, 2)} {character})";
        }
    }
}
