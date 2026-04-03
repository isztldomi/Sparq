using AutoMapper;
using Sparq.DataAccess.Models;
using Sparq.Shared.Models;

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
            CreateMap<User, UserResponseDto>();
            CreateMap<UserRequestDto, User>();
            // CreateMap<Book, BookResponseDto>();
            // CreateMap<BookRequestDto, Book>();

        }
    }
}