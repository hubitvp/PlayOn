namespace PlayOn.Common.Helpers
{
    public class PageResult<T>
    {
        public PageResult(
            IReadOnlyList<T> pageItems,
            int pageMark,
            int pageSize,
            int totalItemsCount)
        {
            PageItems = pageItems;
            PageMark = pageMark;
            PageSize = pageSize;
            TotalItemsCount = totalItemsCount;
        }

        public IReadOnlyList<T> PageItems { get; }

        public int PageMark { get; }

        public int PageSize { get; }

        public int TotalItemsCount { get; }

        public int TotalPages => PageSize <= 0 ? 1 : Math.Max(1, (int)Math.Ceiling(TotalItemsCount / (double)PageSize));

    }
}
