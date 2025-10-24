namespace FinalProjectHome.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public string? Search { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public Dictionary<string, string?> RouteValues() => new()
        {
            ["search"] = Search,
            ["pagesize"] = PageSize.ToString()
        };
    }
}
