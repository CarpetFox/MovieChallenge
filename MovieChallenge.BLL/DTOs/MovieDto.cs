namespace MovieChallenge.BLL.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; } = "";
        public string Overview { get; set; } = "";
        public double? Popularity { get; set; }
        public int? VoteCount { get; set; }
        public double? VoteAverage { get; set; }
        public string OriginalLanguage { get; set; } = "";
        public string PosterUrl { get; set; } = "";
        public List<GenreDto> Genres { get; set; } = [];
    }
}
