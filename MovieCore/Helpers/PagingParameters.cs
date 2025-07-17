namespace MovieCore.Helpers
{
    public class PagingParameters
    {
        private int _pageSize = 10;
        private const int maxPageSize = 100;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }


    }
}
