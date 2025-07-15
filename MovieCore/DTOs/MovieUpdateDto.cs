
using System.ComponentModel.DataAnnotations;

namespace MovieCore.DTOs
{
    public class MovieUpdateDto : MovieBaseDto
    {
        [Required]
        public int GenreId { get; set; }

        [Required]
        public MovieDetailsCreateDto MovieDetails { get; set; } = new ();

        [Required]
        [MinLength(1)]
        public ICollection<ActorWithRoleDto> Actors { get; set; } = [];

    }
}
