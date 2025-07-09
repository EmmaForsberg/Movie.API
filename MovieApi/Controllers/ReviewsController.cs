using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.DTOs;

namespace MovieApi.Controllers
{

    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {

        private readonly MovieContext _context;

        public ReviewsController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet("{movieId}/reviews")]
        public async Task<ActionResult<List<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
                return NotFound();

            var reviews = await _context.Reviews
               .Where(r => r.MovieId == movieId)
               .Select(r => new ReviewDto
               {
                   Name = r.ReviewerName,
                   Comment = r.Comment,
                   Rating = r.Rating
               }).ToListAsync();

            return Ok(reviews);

        }

    }
}
