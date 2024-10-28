using MovieChallenge.BLL.DTOs;

namespace MovieChallenge.BLL.Interfaces
{
    public interface IMovieService
    {
        Task<PaginatedResponse<MovieDto>> GetMovies(PaginatedDataRequest<MovieSearchModel> request);
        Task<IEnumerable<GenreDto>> GetGenres();
    }
}
