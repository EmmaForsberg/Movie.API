using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCore.DTOs
{
    public class MovieBaseDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Range(1900, 2025)]
        public int Year { get; set; }

        [Range(1, 300)]
        public int Duration { get; set; }
    }
}
