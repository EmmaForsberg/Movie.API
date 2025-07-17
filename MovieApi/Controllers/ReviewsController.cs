using Microsoft.AspNetCore.Mvc;
using MovieCore.DTOs;
using MovieCore.Helpers;
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
        public async Task<ActionResult<PagedResult<ReviewDto>>> GetReviewsForMovie([FromQuery] int movieId, [FromQuery] PagingParameters pagingParameters)
        {
            var reviewsPaged = await serviceManager.ReviewService.GetReviewsForMovieAsync(movieId, pagingParameters.PageNumber, pagingParameters.PageSize);

            if (reviewsPaged == null)
                return NotFound(new { message = "Movie not found" });

            return Ok(reviewsPaged);
        }
    }
}
