using AutoMapper;
using MovieContracts;
using MovieCore.DTOs;
using MovieCore.Entities;
using MovieServiceContracts.Service.Contracts;

namespace MovieServices.Services
{
    public class ActorService : IActorService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ActorService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<ActorDto?> GetActorByIdAsync(int id)
        {
            var actor = await uow.ActorRepository.GetAsync(id);
            return actor == null ? null : mapper.Map<ActorDto>(actor);
        }

        public async Task<IEnumerable<ActorDto>> GetAllActorsAsync()
        {
            var actors = await uow.ActorRepository.GetAllAsync();
            return mapper.Map<IEnumerable<ActorDto>>(actors);
        }

        public async Task AddMovieToActorAsync(int actorId, int movieId, MovieActorCreateDto dto)
        {
            var actor = await uow.ActorRepository.GetActorWithMoviesAsync(actorId);
            if (actor == null)
                throw new ArgumentException($"Actor with id {actorId} not found");

            var movie = await uow.MovieRepository.GetAsync(movieId);
            if (movie == null)
                throw new ArgumentException($"Movie with id {movieId} not found");

            if (actor.MovieActors.Any(ma => ma.MovieId == movieId))
                throw new InvalidOperationException("Movie already linked to this actor");

    

            var movieActor = mapper.Map<MovieActor>(dto);
            movieActor.ActorId = actorId;
            movieActor.MovieId = movieId;

            actor.MovieActors.Add(movieActor);

            await uow.CompleteAsync();
        }
    }
}