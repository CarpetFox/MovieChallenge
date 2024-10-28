namespace MovieChallenge.BLL.DTOs
{
    public class PaginatedResponse<T>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public List<T> Data { get; set; } = [];
    }
}
