using MovieCore.DTOs;
using MovieCore.Helpers;

namespace MovieServiceContracts.Service.Contracts
{
    public interface IReviewService
    {
        Task<PagedResult<ReviewDto>> GetReviewsForMovieAsync(int movieId, int pageNumber, int pageSize);
    }
}
