using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Service.Contracts;
using MovieApi.Services;
using MovieCore.DomainContracts;
using MovieCore.DTOs;
using MovieCore.Entities;
using MovieData.Data;
using MovieData.Data.Repositories;

namespace MovieApi.Controllers
{
    [Route("api/actors")]
    [ApiController]
    [Produces("application/json")]
    public class ActorsController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public ActorsController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await serviceManager.ActorService.GetActorByIdAsync(id);
            if (actor == null)
                return NotFound(new { message = "Actor not found" });

            return Ok(actor);
        }

        [HttpPost("{actorId}/movies/{movieId}")]
        public async Task<IActionResult> AddMovieToActor(int actorId, int movieId, [FromBody] MovieActorCreateDto dto)
        {
            try
            {
                await serviceManager.ActorService.AddMovieToActorAsync(actorId, movieId, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

