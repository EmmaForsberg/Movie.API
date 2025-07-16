using MovieCore.DTOs;

namespace MovieApi.Service.Contracts
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetReviewsForMovieAsync(int movieId);
    }
}
