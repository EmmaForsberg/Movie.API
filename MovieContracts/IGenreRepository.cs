using MovieCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieContracts
{
    public interface IGenreRepository
    {
        Task<Genre?> GetGenreByIdAsync(int genreId);
    }
}
