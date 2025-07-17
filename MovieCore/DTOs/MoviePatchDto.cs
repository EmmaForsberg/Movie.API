using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCore.DTOs
{
    public class MoviePatchDto
    {
        public string? Title { get; set; }
        public int? Year { get; set; }
        public int? Duration { get; set; }

        public MovieDetailsPatchDto MovieDetails { get; set; } = new();
    }


}
