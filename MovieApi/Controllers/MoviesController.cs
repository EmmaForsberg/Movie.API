using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.DTOs;
using MovieCore.Entities;
using MovieData.Data;
using MovieData.Data.Repositories;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        //private readonly MovieContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public MoviesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
           // _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _unitOfWork.MovieRepository.GetAllAsync();
            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return Ok(moviesDto);
        }


        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);

            return Ok(movieDto);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetMovieWithDetailsAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            var dto = _mapper.Map<MovieDetailDto>(movie);
            return Ok(dto);
        }


        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDetailDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = _mapper.Map<Movie>(dto);
            movie.MovieActors = new List<MovieActor>();

            var savedMovie = await _unitOfWork.MovieRepository.AddAsync(movie);

            var result = _mapper.Map<MovieDetailDto>(savedMovie);

            return CreatedAtAction(nameof(GetMovieDetails), new { id = result.Id }, result);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieUpdateDto dto)
        {
            var movie = await _unitOfWork.MovieRepository.GetMovieWithDetailsAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            _mapper.Map(dto, movie);

            movie.MovieDetails.Language = dto.MovieDetails.Language;
            movie.MovieDetails.Synopsis = dto.MovieDetails.Synopsis;
            movie.MovieDetails.Budget = dto.MovieDetails.Budget;

            movie.MovieActors.Clear();

            foreach (var actorDto in dto.Actors)
            {
                var actor = await _unitOfWork.ActorRepository
                    .GetByNameAndBirthYearAsync(actorDto.Name, actorDto.BirthYear);

                if (actor == null)
                {
                    actor = new Actor
                    {
                        Name = actorDto.Name,
                        BirthYear = actorDto.BirthYear
                    };
                    _unitOfWork.ActorRepository.Add(actor);
                }

                movie.MovieActors.Add(new MovieActor
                {
                    Actor = actor,
                    Role = actorDto.Role
                });
            }

            _unitOfWork.MovieRepository.Update(movie);

            await _unitOfWork.CompleteAsync(); 

            return NoContent();
        }



        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetMovieForDeleteAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            _unitOfWork.MovieRepository.Remove(movie);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

    }
}
