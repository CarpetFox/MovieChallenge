namespace MovieChallenge.BLL.DTOs
{
    public class MovieSearchModel
    {
        public string? Title { get; set; }
        public List<int>? Genres { get; set; }
        public string? OrderBy { get; set; }
        public bool OrderAscending { get; set; } = true;
    }
}
