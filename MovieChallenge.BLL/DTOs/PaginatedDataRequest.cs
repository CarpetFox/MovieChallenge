namespace MovieChallenge.BLL.DTOs
{
    public class PaginatedDataRequest<T>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string? OrderBy { get; set; }
        public bool OrderAscending { get; set; } = true;
        public T? SearchModel { get; set; }
    }
}
