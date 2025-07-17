using AutoMapper;
using MovieContracts;
using MovieCore.DTOs;
using MovieServiceContracts.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieServices.Services
{
    internal class GenreService : IGenreService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<GenreDto>> GetAllGenresAsync()
        {
            var genres = await _uow.GenreRepository.GetAllAsync();
            return _mapper.Map<List<GenreDto>>(genres);
        }

        public async Task<GenreDto?> GetGenreByIdAsync(int id)
        {
            var genre = await _uow.GenreRepository.GetGenreByIdAsync(id);
            if (genre == null) return null;
            return _mapper.Map<GenreDto>(genre);
        }
    }
}
