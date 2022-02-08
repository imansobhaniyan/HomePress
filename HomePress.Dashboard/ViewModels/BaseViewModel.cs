namespace HomePress.Dashboard.ViewModels
{
    public class BaseViewModel<T>
    {
        public BaseViewModel(int pageSize, int pageNumber, long totalItemsCount, List<T> items)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalItemsCount = (int)totalItemsCount;
            Items = items;

            TotalPagesCount = TotalItemsCount / PageSize + (TotalItemsCount % PageSize == 0 ? 0 : 1);
            From = ((PageNumber - 1) * PageSize) + 1;
            To = Math.Min((PageNumber * PageSize), TotalItemsCount);
        }

        public int TotalItemsCount { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalPagesCount { get; }

        public int From { get; }

        public int To { get; }

        public List<T> Items { get; set; }
    }
}
