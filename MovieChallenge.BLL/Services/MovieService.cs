using Microsoft.EntityFrameworkCore;
using MovieChallenge.BLL.DTOs;
using MovieChallenge.BLL.Extensions;
using MovieChallenge.BLL.Interfaces;
using MovieChallenge.DAL;
using MovieChallenge.DAL.Entities;
using System.Linq.Expressions;

namespace MovieChallenge.BLL.Services
{
    public class MovieService(MovieChallengeContext context) : IMovieService
    {
        public async Task<PaginatedResponse<MovieDto>> GetMovies(PaginatedDataRequest<MovieSearchModel> request)
        {
            var query = context.Movies
                .Include(m => m.Genres)
                .AsQueryable();

            if (request.SearchModel != null)
            {
                if (request.SearchModel.Title.HasValue())
                {
                    query = query.Where(m => m.Title.ToLower().Contains(request.SearchModel.Title!.ToLower()));
                }

                if (request.SearchModel.Genres?.Any() is true)
                {
                    foreach (var id in request.SearchModel.Genres)
                    {
                        query = query.Where(q => q.Genres.Any(g => g.Id == id));
                    }
                }

                if (request.SearchModel.OrderBy.HasValue())
                {
                    Expression<Func<Movie, object>> orderByExpression = m => m.Id;
                    switch (request.SearchModel.OrderBy!.ToLower())
                    {
                        case "title":
                            orderByExpression = m => m.Title.ToLower();
                            break;
                        case "releasedate":
                            orderByExpression = m => m.ReleaseDate!;
                            break;
                    }

                    query = request.SearchModel.OrderAscending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
                }
            }

            //the projection part could be made reusable via something like AutoMapper
            var paginatedResults = await query
                .Paginate(request)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genres = m.Genres.Select(g => new GenreDto
                    {
                        Id = g.Id,
                        Name = g.Name
                    }).ToList(),
                    ReleaseDate = m.ReleaseDate,
                    Overview = m.Overview,
                    PosterUrl = m.PosterUrl,
                    OriginalLanguage = m.OriginalLanguage,
                    Popularity = m.Popularity
                })
                .ToListAsync();

            return new PaginatedResponse<MovieDto>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = await query.CountAsync(),
                Data = paginatedResults
            };
        }

        public async Task<IEnumerable<GenreDto>> GetGenres()
        {
            var genres = await context.Genres
                .Select(g => new GenreDto
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            return genres;
        }
    }
}
