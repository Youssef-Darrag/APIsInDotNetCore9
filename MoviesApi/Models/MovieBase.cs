namespace MoviesApi.Models
{
    public class MovieBase : Base
    {
        [MaxLength(250)]
        public string Title { get; set; } = default!;

        [MaxLength(2500)]
        public string Storeline { get; set; } = default!;
    }
}
