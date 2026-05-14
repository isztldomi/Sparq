// using AutoMapper;
// using Microsoft.AspNetCore.Mvc;
// using Sparq.DataAccess.Models;
// using Sparq.DataAccess.Services;
// 
// namespace Sparq.WebApi.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ParticipantAnswerController : ControllerBase
//     {
// 
//         private readonly IMapper _mapper;
//         private readonly IParticipantAnswerService _participantAnswerService;
//         private readonly IUsersService _usersService;
//         private readonly ISessionService _sessionService;
//         private readonly IParticipantService _participantService;
// 
//         /// <summary>Ctor</summary>
//         /// <param name="mapper">Mapper for DTO mapping</param>
//         /// <param name="participantAnswerService">Participant answer service dependency</param>
//         /// <param name="usersService">User service dependency</param>
//         /// <param name="sessionService">User service dependency</param>
//         /// <param name="participantService">Participant service dependency</param>
//         public ParticipantAnswerController(IMapper mapper, IParticipantAnswerService participantAnswerService,
//             IUsersService usersService, ISessionService sessionService, IParticipantService participantService)
//         {
//             _mapper = mapper;
//             _participantAnswerService = participantAnswerService;
//             _usersService = usersService;
//             _sessionService = sessionService;
//             _participantService = participantService;
//         }
// 
//         [HttpGet("{sessionId}")]
//         public async Task<IActionResult> GetParticipantAnswers([FromRoute] string sessionId)
//         {
//             var user = await _usersService.GetCurrentUserAsync();
// 
//             if (user == null)
//                 return Unauthorized();
// 
//             var session = await _sessionService.ExistsAsync(sessionId);
// 
//             if (!session)
//                 return NotFound();
// 
//             var participant = await _participantService.GetIdByUserIdAndSessionIdAsync(user.Id, sessionId);
// 
//             var answers = await _participantAnswerService.GetByParticipantIdAsync(participant!.Id);
// 
//             return Ok(answers);
//         }
// 
//     }
// }
