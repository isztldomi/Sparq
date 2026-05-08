using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.SnapshotDto;

namespace Sparq.WebApi.Controllers
{
    /// <summary>Snapshot</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SnapshotController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISnapshotService _snapshotService;
        private readonly IUsersService _usersService;
        private readonly IQuizService _quizService;

        /// <summary>Ctor</summary>
        /// <param name="mapper">Mapper for DTO mapping</param>
        /// <param name="snapshotService">Snapshot service dependency</param>
        /// <param name="usersService">User service dependency</param>
        /// <param name="quizService">Quiz service dependency</param>
        public SnapshotController(IMapper mapper, ISnapshotService snapshotService, IUsersService usersService, IQuizService quizService)
        {
            _mapper = mapper;
            _snapshotService = snapshotService;
            _usersService = usersService;
            _quizService = quizService;
        }

        /// <summary>Get by id</summary>
        /// <param name="id">Snapshot identifier</param>
        /// <returns>Snapshot data</returns>
        /// <remarks>Retrieves a snapshot by its unique identifier.</remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SnapshotResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var snapshotEntity = await _snapshotService.GetByIdAsync(id);
            if (snapshotEntity == null)
                return NotFound();
            var response = _mapper.Map<SnapshotResponseDto>(snapshotEntity);
            return Ok(response);
        }

        /// <summary>Get all</summary>
        /// <returns>List of snapshots</returns>
        /// <remarks>Retrieves all available snapshots.</remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SnapshotResponseDto>))]
        public async Task<IActionResult> GetAll()
        {
            var snapshotEntities = await _snapshotService.GetAllAsync();
            var response = _mapper.Map<List<SnapshotResponseDto>>(snapshotEntities);
            return Ok(response);
        }

        /// <summary>Create snapshot</summary>
        /// <param name="snapshotCreateRequestDto">Snapshot creation payload</param>
        /// <returns>Created snapshot</returns>
        /// <remarks>
        /// Creates a new snapshot for a quiz owned by the authenticated user.
        /// The snapshot is persisted and returned with its generated identifier.
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SnapshotResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] SnapshotCreateRequestDto snapshotCreateRequestDto)
        {
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null) 
                return Unauthorized();

            var quiz = await _quizService.GetByIdAsync(snapshotCreateRequestDto!.QuizId);

            if (quiz == null)
                return NotFound("Quiz not found.");

            if (quiz.OwnerId != user.Id)
                return Forbid();

            var snapshotEntity = _mapper.Map<Snapshot>(snapshotCreateRequestDto);

            var createdSnapshot = await _snapshotService.CreateAsync(snapshotEntity);

            var response = _mapper.Map<SnapshotResponseDto>(createdSnapshot);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
    }
}
