using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> _allowedExtensions = new() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1 * 1024 * 1024; // 1 MB

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetAllAsync()
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .Select(m => new MovieDetailsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Rate = m.Rate,
                    Storeline = m.Storeline,
                    Poster = m.Poster,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name
                })
                .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsDto>> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound($"No movie was found with ID: {id}");

            var dto = new MovieDetailsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Rate = movie.Rate,
                Storeline = movie.Storeline,
                Poster = movie.Poster,
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name
            };

            return Ok(dto);
        }

        [HttpGet("GetByGenreId/{genreId}")]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _context.Movies
                .Where(m => m.GenreId == genreId)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .Select(m => new MovieDetailsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Rate = m.Rate,
                    Storeline = m.Storeline,
                    Poster = m.Poster,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name
                })
                .ToListAsync();

            if (movies.Count() == 0)
                return NotFound($"No movies found for genre ID: {genreId}");

            return Ok(movies);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> CreateAsync([FromForm] MovieDto dto)
        {
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");

            using var dataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            Movie movie = new()
            {
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                Storeline = dto.Storeline,
                Poster = dataStream.ToArray(),
                GenreId = dto.GenreId
            };

            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }
    }
}
