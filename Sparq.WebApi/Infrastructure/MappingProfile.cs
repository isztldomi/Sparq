using AutoMapper;
using Sparq.DataAccess.Models;
using Sparq.Shared.Models.AnswerDto;
using Sparq.Shared.Models.QuestionDto;
using Sparq.Shared.Models.QuizDto;
using Sparq.Shared.Models.SnapshotDto;
using Sparq.Shared.Models.UserDto;

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
            CreateMap<User, UserResponseDto>(MemberList.Destination);
            CreateMap<UserRequestDto, User>(MemberList.Destination);

            CreateMap<Quiz, QuizResponseDto>(MemberList.Destination);
            CreateMap<QuizCreateRequestDto, Quiz>(MemberList.Destination);

            CreateMap<Snapshot, SnapshotResponseDto>(MemberList.Destination);
            CreateMap<SnapshotCreateFromQuizRequestDto, Snapshot>(MemberList.Destination);
            CreateMap<SnapshotCreateRequestDto, Snapshot>(MemberList.Destination);

            CreateMap<Question, QuestionResponseDto>(MemberList.Destination);
            CreateMap<QuestionCreateRequestDto, Question>(MemberList.Destination);

            CreateMap<Answer, AnswerResponseDto>(MemberList.Destination);
            CreateMap<AnswerCreateRequestDto, Answer>(MemberList.Destination);
        }
    }
}