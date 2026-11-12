namespace PlayOn.Common.Helpers
{
    public class PageResult<T>
    {
        public PageResult(
            IReadOnlyList<T> items,
            int page,
            int pageSize,
            int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public IReadOnlyList<T> Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public int TotalPages => PageSize <= 0 ? 1 : Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));

        public PageResult<TOut> Map<TOut>(Func<T, TOut> map)
            => new(Items.Select(map).ToList(), Page, PageSize, TotalCount);
    }
}
