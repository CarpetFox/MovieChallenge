using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MovieChallenge.DAL.Entities;
using System.Globalization;
using MovieChallenge.DAL.Models;

namespace MovieChallenge.DAL
{
    public class MovieChallengeContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public string DbPath { get; }

        public MovieChallengeContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "moviechallenge.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity<MovieGenre>();

            try
            {
                //this is only used to generate the migration so can probably be disabled in a prod environment
                SeedDataFromCsv(modelBuilder);
            }
            catch
            {
                //it doesn't matter too much if this fails but ideally should log here
            }
        }

        private void SeedDataFromCsv(ModelBuilder modelBuilder)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null
            };

            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (var reader = new StreamReader($"{dir}\\Seed\\mymoviedb.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<CsvMovieData>().ToList();

                var genres = records.SelectMany(r => r.Genre.Split(",")).Select(r => r.Trim()).Distinct().OrderBy(g => g).ToList();

                var genreEntities = new List<Genre>();
                for (var i = 0; i < genres.Count; i++)
                {
                    genreEntities.Add(new Genre
                    {
                        Id = i + 1,
                        Name = genres[i]
                    });
                }

                modelBuilder.Entity<Genre>(g => g.HasData(genreEntities));

                var movieEntities = new List<Movie>();
                var movieGenreEntities = new List<MovieGenre>();
                for (var i = 0; i < records.Count; i++)
                {
                    var movie = records[i];

                    var moviesGenres = movie.Genre.Split(",").Select(g => g.Trim()).Distinct();
                    var genreEntitiesForMovie = genreEntities.Where(ge => moviesGenres.Contains(ge.Name)).ToList();

                    foreach (var g in genreEntitiesForMovie)
                    {
                        movieGenreEntities.Add(new MovieGenre { GenreId = g.Id, MovieId = i + 1 });
                    }

                    var movieEntity = new Movie
                    {
                        Id = i + 1,
                        OriginalLanguage = movie.OriginalLanguage,
                        Overview = movie.Overview,
                        Popularity = movie.Popularity,
                        PosterUrl = movie.PosterUrl,
                        ReleaseDate = movie.ReleaseDate,
                        Title = movie.Title,
                        VoteAverage = movie.VoteAverage,
                        VoteCount = movie.VoteCount
                    };
                    movieEntities.Add(movieEntity);
                }

                modelBuilder.Entity<Movie>(m => m.HasData(movieEntities));
                modelBuilder.Entity<MovieGenre>(m => m.HasData(movieGenreEntities));
            }
        }
    }

}
