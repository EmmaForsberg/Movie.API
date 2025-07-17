using AutoMapper;
using MovieContracts;
using MovieCore.DTOs;
using MovieCore.Helpers;
using MovieServiceContracts.Service.Contracts;

namespace MovieServices.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ReviewService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<PagedResult<ReviewDto>> GetReviewsForMovieAsync(int movieId, int pageNumber, int pageSize)
        {
            var movieExists = await uow.MovieRepository.AnyAsync(movieId);
            if (!movieExists)
                return null!;

            var totalItems = await uow.ReviewRepository.CountReviewsForMovieAsync(movieId);

            var reviews = await uow.ReviewRepository.GetPagedReviewsForMovieAsync(movieId, pageNumber, pageSize);

            var mappedReviews = mapper.Map<List<ReviewDto>>(reviews);

            return new PagedResult<ReviewDto>(mappedReviews, totalItems, pageNumber, pageSize);
        }

    }
}
