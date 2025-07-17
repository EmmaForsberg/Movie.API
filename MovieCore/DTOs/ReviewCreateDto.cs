using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCore.DTOs
{
    public class ReviewCreateDto
    {
        public int MovieId { get; set; }         // Måste anges
        public int Rating { get; set; }          // T.ex. 1–5
        public string? Comment { get; set; }     // Valfritt
    }
}
