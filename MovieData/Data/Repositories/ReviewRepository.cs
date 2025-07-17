using Microsoft.EntityFrameworkCore;
using MovieContracts;
using MovieCore.Entities;

namespace MovieData.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieContext _context;

        public ReviewRepository(MovieContext context)
        {
            _context = context;
        }


        public async Task<List<Review>> GetReviewsForMovieAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<int> CountReviewsForMovieAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .CountAsync();
        }

        public async Task<List<Review>> GetPagedReviewsForMovieAsync(int movieId, int pageNumber, int pageSize)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .OrderBy(r => r.Id) // Eller annan logik om du har sorteringskrav
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public void Add(Review review)
        {
            _context.Reviews.Add(review);
        }

        public async Task<Review?> GetAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

    }
}
