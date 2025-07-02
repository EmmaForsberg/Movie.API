namespace MovieApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }    = string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; } =  string.Empty;
        public string Duration { get; set; }=   string.Empty;

        //navigation property
        public MovieDetails MovieDetails { get; set; } = null!;

        // 1 - M
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // N-M
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    }
}
