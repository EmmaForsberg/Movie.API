using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class MovieDetails
    {
        [Key]
        public int MovieId { get; set; } //primärnykel och FK

        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Budget { get; set; }

        //Navigation property
        public Movie Movie { get; set; } = null!;
    }
}