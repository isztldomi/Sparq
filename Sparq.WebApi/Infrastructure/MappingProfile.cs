using AutoMapper;

namespace Sparq.WebApi.Infrastructure
{
    /// <summary>
    /// AutoMapper configuration profile for defining object-to-object mappings.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes mapping configurations between domain models and DTOs.
        /// </summary>
        public MappingProfile()
        {
            // Example mappings:
            // CreateMap<Book, BookResponseDto>();
            // CreateMap<BookRequestDto, Book>();

        }
    }
}