using AutoMapper;
using MovieContracts;
using MovieCore.DTOs;
using MovieCore.Entities;
using MovieServiceContracts.Service.Contracts;
using MovieCore.Helpers;

namespace MovieServices.Services
{
    public class MovieService : IMovieService
    {
        private IUnitOfWork uow;
        private readonly IMapper mapper;

        public MovieService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<PagedResult<MovieDto>> GetMoviesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
           var total = await uow.MovieRepository.CountTotalItemsAsync();
            var movies = await uow.MovieRepository.GetPagedMoviesAsync(pageNumber, pageSize);

            var mappedMovies = mapper.Map<List<MovieDto>>(movies);



            return new PagedResult<MovieDto>(mappedMovies, total, pageNumber, pageSize);
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            return movie == null ? null : mapper.Map<MovieDto>(movie);
        }

        public async Task<MovieDetailDto?> GetMovieDetailsAsync(int id)
        {
            var movie = await uow.MovieRepository.GetMovieWithDetailsAsync(id);
            return mapper.Map<MovieDetailDto>(movie);
        }

        public async Task<MovieDetailDto> CreateMovieAsync(MovieCreateDto dto)
        {
            var movie = mapper.Map<Movie>(dto);
            movie.MovieActors = new List<MovieActor>();

            var savedMovie = await uow.MovieRepository.AddAsync(movie);

            return mapper.Map<MovieDetailDto>(savedMovie);
        }

        public async Task<bool> UpdateMovieAsync(int id, MovieUpdateDto dto)
        {
            var movie = await uow.MovieRepository.GetMovieWithDetailsAsync(id);
            if (movie == null) return false;

            mapper.Map(dto, movie);

            movie.MovieDetails.Language = dto.MovieDetails.Language;
            movie.MovieDetails.Synopsis = dto.MovieDetails.Synopsis;
            movie.MovieDetails.Budget = dto.MovieDetails.Budget;

            movie.MovieActors.Clear();

            foreach (var actorDto in dto.Actors)
            {
                var actor = await uow.ActorRepository.GetByNameAndBirthYearAsync(actorDto.Name, actorDto.BirthYear);

                if (actor == null)
                {
                    actor = new Actor
                    {
                        Name = actorDto.Name,
                        BirthYear = actorDto.BirthYear
                    };
                    uow.ActorRepository.Add(actor);
                }

                movie.MovieActors.Add(new MovieActor
                {
                    Actor = actor,
                    Role = actorDto.Role
                });
            }

            uow.MovieRepository.Update(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await uow.MovieRepository.GetMovieForDeleteAsync(id);
            if (movie == null) return false;

            uow.MovieRepository.Remove(movie);
            await uow.CompleteAsync();

            return true;
        }
    }
}
