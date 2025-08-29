namespace MoviesApi.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre?> GetById(byte id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _context.AddAsync(genre);
            await _context.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre> Update(Genre genre)
        {
            _context.Update(genre);
            await _context.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre> Delete(Genre genre)
        {
            _context.Remove(genre);
            await _context.SaveChangesAsync();

            return genre;
        }
    }
}
