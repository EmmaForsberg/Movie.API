using Microsoft.AspNetCore.Mvc;
using MovieContracts;
using MovieCore.DTOs;
using MovieServiceContracts.Service.Contracts;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public MoviesController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var moviesdto = await serviceManager.MovieService.GetMoviesAsync(name, searchQuery,pageNumber,pageSize);

            return Ok(moviesdto);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await serviceManager.MovieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound(new { message = $"Movie with id {id} not found." });
            }

            return Ok(movie);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await serviceManager.MovieService.GetMovieDetailsAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            return Ok(movie);
        }


        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDetailDto>> PostMovie(MovieCreateDto dto)
        {
            var result = await serviceManager.MovieService.CreateMovieAsync(dto);
            return CreatedAtAction(nameof(GetMovieDetails), new { id = result.Id }, result);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieUpdateDto dto)
        {
            var success = await serviceManager.MovieService.UpdateMovieAsync(id, dto);

            if (!success)
                return NotFound(new { message = "Movie not found" });

            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var success = await serviceManager.MovieService.DeleteMovieAsync(id);

            if (!success)
                return NotFound(new { message = "Movie not found" });

            return NoContent();
        }
    }
}
