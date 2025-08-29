namespace MoviesApi.DTOs
{
    public class CreateMovieDto : MovieBase
    {
        public IFormFile Poster { get; set; } = default!;
    }
}
