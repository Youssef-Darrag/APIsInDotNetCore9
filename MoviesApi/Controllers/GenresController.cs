using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllAsync()
        {
            var genres = await _genresService.GetAll();

            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetByIdAsync(byte id)
        {
            var genre = await _genresService.GetById(id);

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

            await _genresService.Create(genre);

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Genre>> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Invalid genre data.");

            var genre = await _genresService.GetById(id);

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genre.Name = dto.Name;

            await _genresService.Update(genre);

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Genre>> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            await _genresService.Delete(genre);

            return Ok(genre);
        }
    }
}
