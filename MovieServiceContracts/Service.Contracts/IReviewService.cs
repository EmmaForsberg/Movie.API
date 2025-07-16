using MovieCore.DTOs;

namespace MovieServiceContracts.Service.Contracts
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetReviewsForMovieAsync(int movieId);
    }
}
