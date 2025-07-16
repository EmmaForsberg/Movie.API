using Microsoft.AspNetCore.Mvc;
using MovieCore.DTOs;
using MovieServiceContracts.Service.Contracts;

namespace MovieApi.Controllers
{

    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {

        private readonly IServiceManager serviceManager;

        public ReviewsController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewDto>>> GetReviewsForMovie([FromQuery] int movieId)
        {
            var reviews = await serviceManager.ReviewService.GetReviewsForMovieAsync(movieId);
            if (reviews == null)
                return NotFound(new { message = "Movie not found" });

            return Ok(reviews);
        }
    }
}
