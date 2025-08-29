using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllAsync()
        {
            var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();

            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetByIdAsync(byte id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            return Ok(genre);
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> CreateAsync([FromBody] GenreDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Invalid genre data.");

            Genre genre = new() { Name = dto.Name };

            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Genre>> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Invalid genre data.");

            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genre.Name = dto.Name;

            await _context.SaveChangesAsync();

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Genre>> DeleteAsync(byte id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            _context.Remove(genre);
            await _context.SaveChangesAsync();

            return Ok(genre);
        }
    }
}
