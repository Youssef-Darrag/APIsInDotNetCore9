namespace MoviesApi.Models
{
    public class Movie : MovieBase
    {
        public int Id { get; set; }

        public byte[] Poster { get; set; } = default!;

        public Genre Genre { get; set; } = default!;
    }
}
