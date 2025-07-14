using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.DTOs;
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

        /// <summary>
        /// Get all movies
        /// </summary>
        /// <returns></returns>
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Genre) 
                .ToListAsync();

            var moviesdto= _mapper.Map<IEnumerable<MovieDto>>(movies);

            return Ok(moviesdto);
        }


        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
             .Where(m => m.Id == id)
             .Select(m => new MovieDto
             {
                 Id = m.Id,
                 Title = m.Title,
                 Year = m.Year,
                 GenreName = m.Genre.Name
             })
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
            var movie = await _context.Movies
     .Include(m => m.Genre)
     .Include(m => m.Reviews)
     .Include(m => m.MovieDetails)
     .Include(m => m.MovieActors)
     .ThenInclude(ma => ma.Actor)
     .Where(m => m.Id == id)
     .Select(m => new MovieDetailDto
     {
         Id = m.Id,
         Title = m.Title,
         Year = m.Year,
         GenreName = m.Genre.Name,
         Synopsis = m.MovieDetails.Synopsis,
         Language = m.MovieDetails.Language,
         Budget = m.MovieDetails.Budget,
         Reviews = m.Reviews.Select(r => new ReviewDto
         {
             Name = r.ReviewerName,
             Rating = r.Rating,
             Comment = r.Comment
         }).ToList(),
         Actors = m.MovieActors.Select(a => new ActorDto
         {
             Name = a.Actor.Name,
             BirthYear = a.Actor.BirthYear,
             Role = a.Role
         }).ToList(),
     })
     .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound(new { message = "Movie not found" });
            }

            return Ok(movie);
        }


        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDetailDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                GenreId = dto.GenreId,
                Duration = dto.Duration,
                MovieDetails = new MovieDetails
                {
                    Language = dto.MovieDetails.Language,
                    Synopsis = dto.MovieDetails.Synopsis,
                    Budget = dto.MovieDetails.Budget
                }
            };

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

            var result = new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                GenreName = (await _context.Genres.FindAsync(movie.GenreId))?.Name ?? "",
                Synopsis = movie.MovieDetails.Synopsis,
                Language = movie.MovieDetails.Language,
                Budget = movie.MovieDetails.Budget,
                Reviews = new List<ReviewDto>(), // tom vid skapande
                Actors = movie.MovieActors.Select(ma => new ActorDto
                {
                    Name = ma.Actor.Name,
                    BirthYear = ma.Actor.BirthYear,
                    Role = ma.Role
                }).ToList()
            };

            return CreatedAtAction(nameof(GetMovieDetails), new { id = movie.Id }, result);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .Include(m => m.MovieActors) // Viktigt att inkludera MovieActors
                    .ThenInclude(ma => ma.Actor) // för att komma åt skådespelarna
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            // Uppdatera filmdata
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Duration = dto.Duration;
            movie.GenreId = dto.GenreId;

            // Uppdatera detaljer
            movie.MovieDetails.Language = dto.MovieDetails.Language;
            movie.MovieDetails.Synopsis = dto.MovieDetails.Synopsis;
            movie.MovieDetails.Budget = dto.MovieDetails.Budget;

            // Rensa gamla kopplingar MovieActors
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
