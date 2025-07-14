using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class MovieDetails
    {
        public int MovieId { get; set; } 

        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Budget { get; set; }

        //Navigation property
        public Movie Movie { get; set; } = null!;
    }
}