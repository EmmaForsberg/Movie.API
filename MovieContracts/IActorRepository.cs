using MovieCore.Entities;

namespace MovieContracts
{
    public interface IActorRepository
    {
        Task<Actor?> GetByNameAndBirthYearAsync(string name, int birthYear);
        void Add(Actor actor);
        Task<Actor?> GetActorWithMoviesAsync(int actorId);
        Task<IEnumerable<Actor>> GetAllAsync();
        Task<Actor?> GetAsync(int id);

    }
}
