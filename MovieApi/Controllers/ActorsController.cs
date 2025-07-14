using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieCore.DTOs;
using MovieCore.Entities;
using MovieData.Data;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class ActorsController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public ActorsController(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddMovieToActor(int actorId, int movieId, [FromBody] MovieActorCreateDto dto)
        {
            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                .FirstOrDefaultAsync(a => a.Id == actorId);
            if (actor == null) return NotFound($"Actor with id {actorId} not found.");

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound($"Movie with id {movieId} not found.");

            if (actor.MovieActors.Any(ma => ma.MovieId == movieId))
                return BadRequest("Movie already linked to this actor.");

            var movieActor = _mapper.Map<MovieActor>(dto);
            movieActor.ActorId = actorId;
            movieActor.MovieId = movieId;

            actor.MovieActors.Add(movieActor);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

