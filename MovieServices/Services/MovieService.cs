﻿using AutoMapper;
using MovieContracts;
using MovieCore.DTOs;
using MovieCore.Entities;
using MovieServiceContracts.Service.Contracts;
using MovieCore.Helpers;
using Microsoft.AspNetCore.JsonPatch;

namespace MovieServices.Services
{
    public class MovieService : IMovieService
    {
        private IUnitOfWork uow;
        private readonly IMapper mapper;

        public MovieService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<PagedResult<MovieDto>> GetMoviesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var total = await uow.MovieRepository.CountTotalItemsAsync(searchQuery);
            var movies = await uow.MovieRepository.GetPagedMoviesAsync(pageNumber, pageSize, searchQuery);

            var mappedMovies = mapper.Map<List<MovieDto>>(movies);

            return new PagedResult<MovieDto>(mappedMovies, total, pageNumber, pageSize);
        }


        public async Task<MovieDto?> GetMovieByIdAsync(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            return movie == null ? null : mapper.Map<MovieDto>(movie);
        }

        public async Task<MovieDetailDto?> GetMovieDetailsAsync(int id)
        {
            var movie = await uow.MovieRepository.GetMovieWithDetailsAsync(id);
            return mapper.Map<MovieDetailDto>(movie);
        }

        public async Task<MovieDetailDto> CreateMovieAsync(MovieCreateDto dto)
        {
            // Kontrollera om en film med samma titel redan finns (case-insensitivt)
            var existingMovie = await uow.MovieRepository.GetMovieByTitleAsync(dto.Title);
            if (existingMovie != null)
            {
                throw new InvalidOperationException($"A movie with the title '{dto.Title}' already exists.");
            }

            // Kontrollera budget
            if (dto.MovieDetails != null & dto.MovieDetails.Budget < 0)
                throw new InvalidOperationException("Budget cannot be negative.");

            // Kontrollera att genren finns
            var genre = await uow.GenreRepository.GetGenreByIdAsync(dto.GenreId);
            if (genre == null)
            {
                // Om genren saknas, returnera null för att indikera fel
                return null;
            }

            // Affärsregel: Dokumentär får inte ha budget över 1 miljon och fler än 10 skådespelare
            if ((genre.Name.ToLower() == "dokumentär" || genre.Name.ToLower() == "documentary"))
            {
                int actorCount = dto.Actors?.Count ?? 0;
                int budget = dto.MovieDetails?.Budget ?? 0;

                if (budget > 1_000_000 && actorCount > 10)
                {
                    throw new InvalidOperationException("A documentary movie cannot have a budget over 1 million and more than 10 actors.");
                }
            }

            var movie = mapper.Map<Movie>(dto);

            movie.GenreId = dto.GenreId;
            movie.Genre = genre;

            movie.MovieActors = new List<MovieActor>();

            var savedMovie = await uow.MovieRepository.AddAsync(movie);

            return mapper.Map<MovieDetailDto>(savedMovie);
        }

        public async Task<bool> UpdateMovieAsync(int id, MovieUpdateDto dto)
        {
            var movie = await uow.MovieRepository.GetMovieWithDetailsAsync(id);
            if (movie == null) return false;

            mapper.Map(dto, movie);

            movie.MovieDetails.Language = dto.MovieDetails.Language;
            movie.MovieDetails.Synopsis = dto.MovieDetails.Synopsis;
            movie.MovieDetails.Budget = dto.MovieDetails.Budget;

            movie.MovieActors.Clear();

            foreach (var actorDto in dto.Actors)
            {
                var actor = await uow.ActorRepository.GetByNameAndBirthYearAsync(actorDto.Name, actorDto.BirthYear);

                if (actor == null)
                {
                    actor = new Actor
                    {
                        Name = actorDto.Name,
                        BirthYear = actorDto.BirthYear
                    };
                    uow.ActorRepository.Add(actor);
                }

                movie.MovieActors.Add(new MovieActor
                {
                    Actor = actor,
                    Role = actorDto.Role
                });
            }

            uow.MovieRepository.Update(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await uow.MovieRepository.GetMovieForDeleteAsync(id);
            if (movie == null) return false;

            uow.MovieRepository.Remove(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<Movie?> GetMovieWithDetailsAsync(int id)
        {
            return await uow.MovieRepository.GetMovieWithDetailsAsync(id);
        }

        public async Task<bool> PatchMovieAsync(int id, MoviePatchDto patchedDto)
        {
            var movie = await uow.MovieRepository.GetMovieWithDetailsAsync(id);
            if (movie == null)
                return false;

            // Mappa patchad DTO tillbaka till entity
            mapper.Map(patchedDto, movie);

            // Gör eventuella affärsregler/kontroller här (ex. budget ej negativ)

            uow.MovieRepository.Update(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<MoviePatchDto?> GetMoviePatchDtoAsync(int id)
        {
            var movie = await uow.MovieRepository.GetMovieWithDetailsAsync(id);
            if (movie == null)
                return null;

            return mapper.Map<MoviePatchDto>(movie);
        }
    }
}
