using System.ComponentModel.DataAnnotations;

namespace MovieCore.DTOs
{
    public class MovieDto : MovieBaseDto
    {
        public int Id { get; set; }
        public string GenreName { get; set; } = string.Empty;
    }
}
