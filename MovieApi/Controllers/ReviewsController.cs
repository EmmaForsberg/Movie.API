using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DTOs;
using MovieData.Data;

namespace MovieApi.Controllers
{

    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {

        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public ReviewsController(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
                return NotFound();

            var reviews = await _context.Reviews
               .Where(r => r.MovieId == movieId)
               .ToListAsync();

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

            return Ok(reviewDtos);
        }


    }
}
