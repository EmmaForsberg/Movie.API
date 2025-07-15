
using System.ComponentModel.DataAnnotations;

namespace MovieCore.DTOs
{
    public class MovieCreateDto : MovieBaseDto
    {
        [Required]
        public int GenreId { get; set; }

        [Required]
        public MovieDetailsCreateDto MovieDetails { get; set; } = new MovieDetailsCreateDto();

        [Required]
        [MinLength(1)]
        public ICollection<ActorWithRoleDto> Actors { get; set; } = [];

    }
}
