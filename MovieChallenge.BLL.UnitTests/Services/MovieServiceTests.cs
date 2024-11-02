using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MovieChallenge.BLL.DTOs;
using MovieChallenge.BLL.Services;
using MovieChallenge.DAL;

namespace MovieChallenge.BLL.UnitTests.Services
{
    public class MovieServiceTests
    {
        private readonly SqliteConnection _connection;
        private readonly MovieService _movieService;
        private readonly MovieChallengeContext _context;


        public MovieServiceTests()
        {
            // I chose to use an in memory DB over mocking the db context as it allows simulation of EF queries
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var contextOptions = new DbContextOptionsBuilder<MovieChallengeContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new MovieChallengeContext(contextOptions);
            _context.Database.EnsureCreated();
            _movieService = new MovieService(_context);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public async Task GetMovies_PaginatesDataToCorrectPageSize(int pageSize)
        {
            var result = await _movieService.GetMovies(new PaginatedDataRequest<MovieSearchModel>
            {
                Page = 0,
                PageSize = pageSize
            });

            Assert.That(pageSize, Is.EqualTo(result.Data.Count));
        }

        [TestCase("batman")]
        [TestCase("superman")]
        [TestCase("spider-man")]
        public async Task GetMovies_FiltersByTitle(string title)
        {
            var result = await _movieService.GetMovies(new PaginatedDataRequest<MovieSearchModel>
            {
                Page = 0,
                PageSize = 10,
                SearchModel = new ()
                {
                    Title = title
                }
            });

            Assert.IsNotEmpty(result.Data);
            foreach (var movie in result.Data)
            {
                Assert.IsTrue(movie.Title.Contains(title, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        [TestCase(new int[] { 4 })]
        [TestCase(new int[] { 1, 2 })]
        [TestCase(new int[] { 3, 7 })]
        public async Task GetMovies_FiltersByGenres(int[] genreIds)
        {
            var result = await _movieService.GetMovies(new PaginatedDataRequest<MovieSearchModel>
            {
                Page = 0,
                PageSize = 10,
                SearchModel = new()
                {
                    Genres = genreIds.ToList()
                }
            });

            foreach (var movie in result.Data)
            {
                var genreIdsForMovie = movie.Genres.Select(g => g.Id).ToList();

                foreach (var id in genreIds)
                {
                    Assert.Contains(id, genreIdsForMovie);
                }
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetMovies_OrdersByTitle(bool orderAscending)
        {
            var result = await _movieService.GetMovies(new PaginatedDataRequest<MovieSearchModel>
            {
                Page = 1,
                PageSize = 1000,
                OrderBy = "title",
                OrderAscending = orderAscending,
                SearchModel = new()
            });

            MovieDto? previous = null;
            foreach (var movie in result.Data)
            {
                // C# and SQL compare special characters differently, so I've ignored these as a heuristic
                // would be more efficient with a regex, but I struggled to find one that can deal with hispanic accents in C#
                if (!movie.Title.All(char.IsLetterOrDigit)) continue;

                if (previous != null)
                {
                    if (orderAscending)
                    {
                        Assert.LessOrEqual(previous.Title.ToLower(), movie.Title.ToLower());
                    }
                    else
                    {
                        Assert.GreaterOrEqual(previous.Title.ToLower(), movie.Title.ToLower());
                    }
                }

                previous = movie;
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetMovies_OrdersByReleaseDate(bool orderAscending)
        {
            var result = await _movieService.GetMovies(new PaginatedDataRequest<MovieSearchModel>
            {
                Page = 1,
                PageSize = 1000,
                OrderBy = "releasedate",
                OrderAscending = orderAscending,
                SearchModel = new()
            });

            MovieDto? previous = null;
            foreach (var movie in result.Data)
            {

                if (previous != null)
                {
                    if (orderAscending)
                    {
                        Assert.LessOrEqual(previous.ReleaseDate, movie.ReleaseDate);
                    }
                    else
                    {
                        Assert.GreaterOrEqual(previous.ReleaseDate, movie.ReleaseDate);
                    }
                }

                previous = movie;
            }
        }

        [Test]
        public async Task GetGenres_GetsAllGenres()
        {
            var result = await _movieService.GetGenres();
            var resultGenreNames = result.Select(r => r.Name).ToList();

            foreach (var genre in _context.Genres) 
            {
                Assert.Contains(genre.Name, resultGenreNames);
            }
        }
    }
}
