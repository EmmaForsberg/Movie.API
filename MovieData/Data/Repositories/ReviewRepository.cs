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
    }
}
