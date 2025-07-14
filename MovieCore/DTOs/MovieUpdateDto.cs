
using System.ComponentModel.DataAnnotations;

namespace MovieCore.DTOs
{
    public class MovieUpdateDto
    {
        public int Id { get; set; } 

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Range(1900, 2025)]
        public int Year { get; set; }

        [Range(1, 300)]
        public int Duration { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public MovieDetailsCreateDto MovieDetails { get; set; } = new MovieDetailsCreateDto();

        [Required]
        [MinLength(1)]
        public List<ActorWithRoleDto> Actors { get; set; } = new();

    }
}
