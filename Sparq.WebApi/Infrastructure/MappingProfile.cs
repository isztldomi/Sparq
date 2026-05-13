using AutoMapper;
using Sparq.DataAccess.Models;
using Sparq.Shared.Models.AnswerDto;
using Sparq.Shared.Models.Participant;
using Sparq.Shared.Models.QuestionDto;
using Sparq.Shared.Models.QuizDto;
using Sparq.Shared.Models.SessionDto;
using Sparq.Shared.Models.SessionQuestion;
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

            CreateMap<Quiz, MyQuizListDto>(MemberList.Destination);
            CreateMap<Quiz, QuizResponseDto>(MemberList.Destination);
            CreateMap<QuizCreateRequestDto, Quiz>(MemberList.Destination);

            CreateMap<Snapshot, SnapshotMetaDetails2ResponseDto>(MemberList.Destination);
            CreateMap<Snapshot, SnapshotMetaDetailsResponseDto>(MemberList.Destination);
            CreateMap<Snapshot, MyQuizSnapshotListDto>(MemberList.Destination);
            CreateMap<Snapshot, SnapshotResponseDto>(MemberList.Destination);
            CreateMap<SnapshotCreateFromQuizRequestDto, Snapshot>(MemberList.Destination);
            CreateMap<SnapshotCreateRequestDto, Snapshot>(MemberList.Destination);

            CreateMap<Session, SessionPublicWaitingListDto>(MemberList.Destination);
            CreateMap<Session, SessionListDto>(MemberList.Destination);
            CreateMap<Session, CreateSessionResponseDto>(MemberList.Destination);

            CreateMap<SessionQuestionState, CurrentSessionQuestionStateWithoutResultDto>(MemberList.Destination);
            CreateMap<SessionQuestionState, CurrentSessionQuestionStateWithResultDto>(MemberList.Destination);

            CreateMap<Question, CurrentQuestionWithoutResultDto>(MemberList.Destination);
            CreateMap<Question, CurrentQuestionWithResultDto>(MemberList.Destination);
            CreateMap<Question, QuestionResponseDto>(MemberList.Destination);
            CreateMap<QuestionCreateRequestDto, Question>()
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.Snapshot, opt => opt.Ignore());

            CreateMap<Answer, CurrentQuestionAnswerWithoutResultDto>(MemberList.Destination);
            CreateMap<Answer, CurrentQuestionAnswerWithResultDto>(MemberList.Destination);
            CreateMap<Answer, AnswerResponseDto>(MemberList.Destination);
            CreateMap<AnswerCreateRequestDto, Answer>(MemberList.Destination);

            CreateMap<Participant, ParticipantPublicListResponseDto>(MemberList.Destination);
        }
    }
}