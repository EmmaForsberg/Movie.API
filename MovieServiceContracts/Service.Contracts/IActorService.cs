using MovieCore.DTOs;

namespace MovieServiceContracts.Service.Contracts
{
    public interface IActorService
    {
        Task<ActorDto?> GetActorByIdAsync(int id);
        Task<IEnumerable<ActorDto>> GetAllActorsAsync();
        Task AddMovieToActorAsync(int actorId, int movieId, MovieActorCreateDto dto);

    }
}
