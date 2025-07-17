using Microsoft.EntityFrameworkCore;
using MovieContracts;
using MovieCore.Entities;

namespace MovieData.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;

        public MovieRepository(MovieContext context)
        {
            _context = context;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var savedMovie = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.MovieDetails)
                .Include(m => m.Reviews)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == movie.Id);

            if (savedMovie == null)
            {
                throw new InvalidOperationException("Failed to retrieve the saved movie.");
            }

            return savedMovie;
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Movies.AnyAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie?> GetAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.MovieDetails)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie?> GetMovieWithDetailsAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Reviews)
                .Include(m => m.MovieDetails)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
        }

        public async Task<Movie?> GetMovieForDeleteAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.MovieDetails)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Remove(Movie movie)
        {
            _context.Movies.Remove(movie);
        }
        public async Task<List<Movie>> GetPagedMoviesAsync(int pageNumber, int pageSize, string? searchQuery)
        {
            var query = _context.Movies.Include(m => m.Genre).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(m => m.Title.ToLower().Contains(searchQuery.ToLower()));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<int> CountTotalItemsAsync(string? searchQuery)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(m => m.Title.ToLower().Contains(searchQuery.ToLower()));
            }

            return await query.CountAsync();
        }

    }
}
