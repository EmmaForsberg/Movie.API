using System.ComponentModel.DataAnnotations;

namespace MovieCore.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Year { get; set; }

        public string GenreName { get; set; } = string.Empty;
    }
}
