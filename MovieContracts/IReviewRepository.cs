using MovieCore.DTOs;
using MovieCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieContracts
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsForMovieAsync(int movieId);
        Task<int> CountReviewsForMovieAsync(int movieId);
        Task<List<Review>> GetPagedReviewsForMovieAsync(int movieId, int pageNumber, int pageSize);
        void Add(Review review);

        Task<Review?> GetAsync(int id);
    }


}
