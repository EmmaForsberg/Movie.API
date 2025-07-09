using Microsoft.AspNetCore.Mvc;
using MovieApi.Data;
using MovieApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly MovieContext _context;

        public ActorsController(MovieContext context)
        {
            _context = context;
        }

        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddMovieToActor(int actorId, int movieId, [FromBody] string role)
        {
            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                .FirstOrDefaultAsync(a => a.Id == actorId);
            if (actor == null) return NotFound($"Actor with id {actorId} not found.");

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound($"Movie with id {movieId} not found.");

            if (actor.MovieActors.Any(ma => ma.MovieId == movieId))
                return BadRequest("Movie already linked to this actor.");

            actor.MovieActors.Add(new MovieActor
            {
                ActorId = actorId,
                MovieId = movieId,
                Role = role ?? ""
            });

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

