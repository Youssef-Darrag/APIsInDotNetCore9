namespace MoviesApi.DTOs
{
    public class MovieDetailsDto : Base
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Storeline { get; set; } = string.Empty;

        public byte[] Poster { get; set; } = default!;

        public string GenreName { get; set; } = default!;
    }
}
