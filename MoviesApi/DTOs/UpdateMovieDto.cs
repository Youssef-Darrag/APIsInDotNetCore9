namespace MoviesApi.DTOs
{
    public class UpdateMovieDto : MovieBase
    {
        public IFormFile? Poster { get; set; } = default!;
    }
}
