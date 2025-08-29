namespace MoviesApi.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre?> GetById(byte id);
        Task<Genre> Create(Genre genre);
        Task<Genre> Update(Genre genre);
        Task<Genre> Delete(Genre genre);
    }
}
