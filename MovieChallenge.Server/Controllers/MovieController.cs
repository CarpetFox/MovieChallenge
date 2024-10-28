using Microsoft.AspNetCore.Mvc;
using MovieChallenge.BLL.DTOs;
using MovieChallenge.BLL.Exceptions;
using MovieChallenge.BLL.Interfaces;

namespace MovieChallenge.Server.Controllers
{
    //may be good to add authentication if going into production
    [Route("[controller]")]
    public class MovieController(ILogger<MovieController> logger, IMovieService movieService) : ControllerBase
    {
        [HttpGet]
        public async Task<PaginatedResponse<MovieDto>> Get([FromQuery] PaginatedDataRequest<MovieSearchModel> request)
        {
            if (request.SearchModel?.Title == "throw custom exception") throw new UiFriendlyException("This is an error with a custom message!");
            if (request.SearchModel?.Title == "throw exception") throw new Exception();

            return await movieService.GetMovies(request);
        }

        [HttpGet(nameof(GetGenres))]
        public async Task<IEnumerable<GenreDto>> GetGenres()
        {
            return await movieService.GetGenres();
        }
    }
}
