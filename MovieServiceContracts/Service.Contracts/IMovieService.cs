using MovieCore.DTOs;

namespace MovieServiceContracts.Service.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetMoviesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<MovieDto?> GetMovieByIdAsync(int id);
        Task<MovieDetailDto?> GetMovieDetailsAsync(int id);
        Task<MovieDetailDto> CreateMovieAsync(MovieCreateDto dto);
        Task<bool> UpdateMovieAsync(int id, MovieUpdateDto dto);
        Task<bool> DeleteMovieAsync(int id);
    }
}
