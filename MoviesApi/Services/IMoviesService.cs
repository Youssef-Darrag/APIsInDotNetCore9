namespace MoviesApi.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAll(byte genreId = 0);
        Task<Movie?> GetById(int id);
        Task<Movie> Create(Movie movie);
        Task<Movie> Update(Movie movie);
        Task<Movie> Delete(Movie movie);
    }
}
