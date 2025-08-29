using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;

        private readonly List<string> _allowedExtensions = new() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1 * 1024 * 1024; // 1 MB

        public MoviesController(IMoviesService moviesService, IGenresService genresService)
        {
            _moviesService = moviesService;
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();

            if (movies.Count() == 0)
                return NotFound("No movies found.");

            // TODO: Map Movies to DTO.

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsDto>> GetByIdAsync(int id)
        {
            var movie = await _moviesService.GetById(id);

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
            var movies = await _moviesService.GetAll(genreId);

            if (movies.Count() == 0)
                return NotFound($"No movies found for genre ID: {genreId}");

            // TODO: Map Movies to DTO.

            return Ok(movies);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> CreateAsync([FromForm] CreateMovieDto dto)
        {
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);

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

            await _moviesService.Create(movie);

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> UpdateAsync(int id, [FromForm] UpdateMovieDto dto)
        {
            var movie = await _moviesService.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found with ID: {id}");

            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");

            // If a new poster is provided, validate and process it
            if (dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.Storeline = dto.Storeline;
            movie.GenreId = dto.GenreId;

            await _moviesService.Update(movie);

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteAsync(int id)
        {
            var movie = await _moviesService.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found with ID: {id}");

            await _moviesService.Delete(movie);

            return Ok(movie);
        }
    }
}
