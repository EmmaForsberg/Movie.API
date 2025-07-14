using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DTOs;
using MovieCore.Entities;
using MovieData.Data;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;


        public MoviesController(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                 .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            //var moviesdto = _mapper.Map<IEnumerable<MovieDto>>(movies);

            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
                   .Where(m => m.Id == id)
                   .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            } 

            return Ok(movie);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var dto = await _context.Movies
                .Where(m => m.Id == id)
                .ProjectTo<MovieDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (dto == null)
                return NotFound();

            return Ok(dto);
        }


        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDetailDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = _mapper.Map<Movie>(dto);

            movie.MovieActors = new List<MovieActor>();

            foreach (var actorDto in dto.Actors)
            {
                var actor = await _context.Actors
                    .FirstOrDefaultAsync(a => a.Name == actorDto.Name && a.BirthYear == actorDto.BirthYear);

                if (actor == null)
                {
                    actor = new Actor
                    {
                        Name = actorDto.Name,
                        BirthYear = actorDto.BirthYear
                    };
                    _context.Actors.Add(actor);
                }

                movie.MovieActors.Add(new MovieActor
                {
                    Actor = actor,
                    Role = actorDto.Role
                });
            }

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Hämta igen för att säkerställa att navigation properties är laddade
            var savedMovie = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.MovieDetails)
                .Include(m => m.Reviews)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == movie.Id);

            var result = _mapper.Map<MovieDetailDto>(savedMovie);

            return CreatedAtAction(nameof(GetMovieDetails), new { id = result.Id }, result);
        }



        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            // Mappa enkla egenskaper direkt från dto till movie
            _mapper.Map(dto, movie);

            // Uppdatera MovieDetails (kan mappas också om du vill)
            movie.MovieDetails.Language = dto.MovieDetails.Language;
            movie.MovieDetails.Synopsis = dto.MovieDetails.Synopsis;
            movie.MovieDetails.Budget = dto.MovieDetails.Budget;

            // Uppdatera MovieActors (manuellt eftersom det är en join-tabell)
            movie.MovieActors.Clear();

            foreach (var actorDto in dto.Actors)
            {
                var actor = await _context.Actors
                    .FirstOrDefaultAsync(a => a.Name == actorDto.Name && a.BirthYear == actorDto.BirthYear);

                if (actor == null)
                {
                    actor = new Actor
                    {
                        Name = actorDto.Name,
                        BirthYear = actorDto.BirthYear
                    };
                    _context.Actors.Add(actor);
                }

                movie.MovieActors.Add(new MovieActor
                {
                    Actor = actor,
                    Role = actorDto.Role
                });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies
    .Include(m => m.MovieDetails)
    .Include(m => m.MovieActors)
    .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound((new { message = "Movie not found" }));
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
