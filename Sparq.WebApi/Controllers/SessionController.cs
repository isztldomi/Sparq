using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Page;
using Sparq.Shared.Models.SessionDto;

namespace Sparq.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        private readonly IUsersService _usersService;
        private readonly IQuizService _quizService;
        private readonly ISnapshotService _snapshotService;
        public SessionController(IMapper mapper, ISessionService sessionService, IUsersService usersService, IQuizService quizService, ISnapshotService snapshotService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _usersService = usersService;
            _quizService = quizService;
            _snapshotService = snapshotService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateSessionResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateSessionRequestDto dto)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var quiz = await _quizService.GetByIdAsync(dto.QuizId);

            if (quiz == null)
                return NotFound("Quiz not found");

            if (quiz.OwnerId != user.Id)
                return Forbid();

            var snapshot = await _snapshotService.GetByIdAsync(quiz.LastSnapshot!.Id);

            if (snapshot == null)
                return NotFound("Snapshot not found");

            var session = await _sessionService.CreateAsync(snapshot.Id);

            if (session == null) 
                return NotFound("Snapshot not found");

            var result = _mapper.Map<CreateSessionResponseDto>(session);

            return Ok(result);
        }
    }
}
