using System.ComponentModel.DataAnnotations;

namespace MovieCore.DTOs
{
    public class MovieDetailDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Year { get; set; }

        public string GenreName { get; set; } = string.Empty;

        public string Synopsis { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public int Budget { get; set; }

        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();

        public ICollection<ActorDto> Actors { get; set; } = new List<ActorDto>();
    }
}
