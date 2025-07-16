using MovieCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCore.DomainContracts
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
