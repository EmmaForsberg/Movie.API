using Microsoft.AspNetCore.Mvc;
using MovieCore.DTOs;
using MovieCore.Helpers;
using MovieServiceContracts.Service.Contracts;

namespace MoviePresentation.Controllers
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

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReview(ReviewCreateDto dto)
        {
            try
            {
                var created = await serviceManager.ReviewService.CreateReviewAsync(dto);
                return CreatedAtAction(nameof(GetReview), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    title = "Business Rule Violation",
                    detail = ex.Message,
                    status = 400
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await serviceManager.ReviewService.GetAsync(id);

            if (review == null)
                return NotFound();

            return Ok(review);
        }

    }
}
