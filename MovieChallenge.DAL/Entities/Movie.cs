using Microsoft.EntityFrameworkCore;

namespace MovieChallenge.DAL.Entities
{
    [Index(nameof(Title))]
    public class Movie
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
        public List<Genre> Genres { get; } = [];
        public List<MovieGenre> MovieGenres { get; set; } = [];
    }
}
