using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Participant;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Media</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IParticipantService _participantService;
        private readonly IUsersService _usersService;

        public ParticipantController(IMapper mapper, IParticipantService participantService, IUsersService usersService)
        {
            _mapper = mapper;
            _participantService = participantService;
            _usersService = usersService;
        }

        [HttpGet("{sessionId}/is-joined")]
        [Authorize]
        public async Task<IActionResult> IsJoined([FromRoute] string sessionId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var isJoined = await _participantService.IsUserJoinedAsync(user.Id, sessionId);

            ParticipantIsJoinedResponseDto result = new ParticipantIsJoinedResponseDto { IsJoined = isJoined };

            return Ok(result);
        }

        [HttpGet("{sessionId}/ext-user-is-joined/{extUserId}")]
        public async Task<IActionResult> IsJoined([FromRoute] string sessionId, [FromRoute] string extUserId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user != null)
                return Forbid();

            var isJoined = await _participantService.IsExtUserJoinedAsync(extUserId, sessionId);

            ParticipantIsJoinedResponseDto result = new ParticipantIsJoinedResponseDto { IsJoined = isJoined };

            return Ok(result);
        }
    }
}
