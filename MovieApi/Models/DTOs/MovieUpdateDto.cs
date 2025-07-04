using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTOs
{
    public class MovieUpdateDto
    {
        //nästan samma som moviecreatedto men kan ha en id óm du inte sätter det i urlen. samma valideringar som create
        //Dto för post

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
        public List<int> ActorIds { get; set; } = new();
    }
}
