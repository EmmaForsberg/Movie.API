using AutoMapper;
using MovieApi.Service.Contracts;
using MovieCore.DomainContracts;
using MovieCore.DTOs;

namespace MovieApi.Services
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

        public async Task<List<ReviewDto>> GetReviewsForMovieAsync(int movieId)
        {
            var movieExists = await uow.MovieRepository.AnyAsync(movieId);
            if (!movieExists)
                return null!;

            var reviews = await uow.ReviewRepository.GetReviewsForMovieAsync(movieId);
            return mapper.Map<List<ReviewDto>>(reviews);
        }
    }
}
