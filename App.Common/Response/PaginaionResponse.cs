namespace App.Common.Response
{
    public class PaginationResponse<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public IEnumerable<T> Result { get; set; } = Enumerable.Empty<T>();
    }
}
