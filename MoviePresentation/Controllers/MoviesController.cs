﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieCore.DTOs;
using MovieCore.Helpers;
using MovieServiceContracts.Service.Contracts;

namespace  MoviePresentation.Controllers

{
    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public MoviesController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<MovieDto>>> GetMovies(
      [FromQuery] string? name,
      [FromQuery] string? searchQuery,
      [FromQuery] PagingParameters pagingParameters)
        {
            var moviesDto = await serviceManager.MovieService.GetMoviesAsync(
                name,
                searchQuery,
                pagingParameters.PageNumber,
                pagingParameters.PageSize);

            return Ok(moviesDto);
        }


        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await serviceManager.MovieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound(new { message = $"Movie with id {id} not found." });
            }

            return Ok(movie);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await serviceManager.MovieService.GetMovieDetailsAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            return Ok(movie);
        }


        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDetailDto>> PostMovie(MovieCreateDto dto)
        {
            var result = await serviceManager.MovieService.CreateMovieAsync(dto);
            if (result == null)
            {
                // Returnera ProblemDetails med 400 Bad Request och förklaring
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Genre",
                    Detail = $"Genre with id {dto.GenreId} does not exist."
                };

                return BadRequest(problemDetails);
            }
                return CreatedAtAction(nameof(GetMovieDetails), new { id = result.Id }, result);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieUpdateDto dto)
        {
            var success = await serviceManager.MovieService.UpdateMovieAsync(id, dto);

            if (!success)
                return NotFound(new { message = "Movie not found" });

            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var success = await serviceManager.MovieService.DeleteMovieAsync(id);

            if (!success)
                return NotFound(new { message = "Movie not found" });

            return NoContent();
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchMovie(int id, JsonPatchDocument<MoviePatchDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var movieToPatch = await serviceManager.MovieService.GetMoviePatchDtoAsync(id);
            if (movieToPatch == null)
                return NotFound();

            patchDoc.ApplyTo(movieToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await serviceManager.MovieService.PatchMovieAsync(id, movieToPatch);
            if (!success)
                return NotFound();

            return NoContent();
        }

    }
}
