namespace MovieCore.DTOs
{ 
    public class ReviewDto
    {
        public string Name { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public int Rating { get; set; }
    }
}