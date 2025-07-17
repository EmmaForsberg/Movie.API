using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCore.DTOs;
using MovieServiceContracts.Service.Contracts;

namespace MovieApi.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GenresController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDto>>> GetGenres()
        {
            var genres = await _serviceManager.GenreService.GetAllGenresAsync();
            return Ok(genres);
        }
    }
}
