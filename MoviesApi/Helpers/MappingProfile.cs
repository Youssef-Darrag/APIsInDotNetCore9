namespace MoviesApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(dest => dest.Poster, opt => opt.Ignore());
        }
    }
}
