using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        [Range(1,5, ErrorMessage = "Raiting måste vara mellan 1 och 5.")]
        public int Rating { get; set; }

        //Foreign key
        public int MovieId { get; set; }

        //Navigation propert
        public Movie Movie { get; set; }


    }
}
