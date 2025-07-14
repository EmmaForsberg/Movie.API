using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTOs
{
    public class MovieDetailsCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Synopsis { get; set; } =string.Empty;

        [Required]
        [StringLength(50)]
        public string Language { get; set; } = string.Empty;

        [Range(100000, 100000000)]
        public int Budget { get; set; }
    }
}