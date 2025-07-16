using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.DTOs;
using MovieData.Data;
using MovieData.Data.Repositories;

namespace MovieApi.Controllers
{

    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {

        //private readonly MovieContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewsController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var movieExists = await _unitOfWork.MovieRepository.AnyAsync(movieId);
            if (!movieExists)
                return NotFound(new { message = "Movie not found" });

            var reviews = await _unitOfWork.ReviewRepository.GetReviewsForMovieAsync(movieId);
            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

            return Ok(reviewDtos);
        }

    }
}
