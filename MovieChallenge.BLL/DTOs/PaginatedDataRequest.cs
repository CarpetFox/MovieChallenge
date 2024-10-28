namespace MovieChallenge.BLL.DTOs
{
    public class PaginatedDataRequest<T>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public T? SearchModel { get; set; }
    }
}
