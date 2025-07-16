using MovieCore.Entities;

namespace MovieContracts
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie?> GetAsync(int id);
        Task<bool> AnyAsync(int id);

        Task<Movie?> GetMovieWithDetailsAsync(int id);
        Task<Movie> AddAsync(Movie movie);
        void Update(Movie movie);
        Task<Movie?> GetMovieForDeleteAsync(int id);
        void Remove(Movie movie);

    }
}
