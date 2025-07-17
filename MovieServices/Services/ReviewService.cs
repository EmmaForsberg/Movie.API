using AutoMapper;
using MovieContracts;
using MovieCore.DTOs;
using MovieCore.Entities;
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

        public async Task<ReviewDto> CreateReviewAsync(ReviewCreateDto dto)
        {
            var movie = await uow.MovieRepository.GetAsync(dto.MovieId);
            if (movie == null)
                throw new InvalidOperationException($"Movie with id {dto.MovieId} does not exist.");

            // Affärsregel: max 10 recensioner
            if (movie.Reviews.Count >= 10)
                throw new InvalidOperationException("A movie can have at most 10 reviews.");

            var review = mapper.Map<Review>(dto);
            uow.ReviewRepository.Add(review);
            await uow.SaveAsync();

            return mapper.Map<ReviewDto>(review);
        }

        public async Task<ReviewDto?> GetAsync(int id)
        {
            var review = await uow.ReviewRepository.GetAsync(id);
            if (review == null) return null;
            return mapper.Map<ReviewDto>(review);
        }


    }
}
