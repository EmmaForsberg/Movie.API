using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
