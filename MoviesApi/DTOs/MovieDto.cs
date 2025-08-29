namespace MoviesApi.DTOs
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; } = default!;

        public int Year { get; set; }

        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Storeline { get; set; } = default!;

        public IFormFile Poster { get; set; } = default!;

        public byte GenreId { get; set; }
    }
}
