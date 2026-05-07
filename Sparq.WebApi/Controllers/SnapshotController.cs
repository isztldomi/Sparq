using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.SnapshotDto;

namespace Sparq.WebApi.Controllers
{
    /// <summary>
    /// Handles snapshot-related operations such as creation and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SnapshotController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISnapshotService _snapshotService;
        private readonly IUsersService _usersService;
        private readonly IQuizService _quizService;


        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance for DTO-entity conversions.</param>
        /// <param name="snapshotService">Service handling snapshot business logic.</param>
        public SnapshotController(IMapper mapper, ISnapshotService snapshotService, IUsersService usersService, IQuizService quizService)
        {
            _mapper = mapper;
            _snapshotService = snapshotService;
            _usersService = usersService;
            _quizService = quizService;
        }

        /// <summary>
        /// Snapshot by Id
        /// </summary>
        /// <param name="id">The snapshot identifier.</param>
        /// <returns>The snapshot if found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SnapshotResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var snapshotEntity = await _snapshotService.GetByIdAsync(id);
            if (snapshotEntity == null)
                return NotFound();
            var response = _mapper.Map<SnapshotResponseDto>(snapshotEntity);
            return Ok(response);
        }

        /// <summary>
        /// All snapshots
        /// </summary>
        /// <returns>A list of snapshots.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SnapshotResponseDto>))]
        public async Task<IActionResult> GetAll()
        {
            var snapshotEntities = await _snapshotService.GetAllAsync();
            var response = _mapper.Map<List<SnapshotResponseDto>>(snapshotEntities);
            return Ok(response);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="snapshotCreateRequestDto">The snapshot creation payload.</param>
        /// <returns>The created snapshot.</returns>
        /// <remarks>
        /// The snapshot is persisted and returned with its generated identifier.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SnapshotResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] SnapshotCreateRequestDto snapshotCreateRequestDto)
        {
            // aktuális user
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            // quiz lekérése
            var quiz = await _quizService.GetByIdAsync(snapshotCreateRequestDto.QuizId);

            if (quiz == null)
                return NotFound("Quiz not found.");

            // ownership check
            if (quiz.OwnerId != user.Id)
                return Forbid();


            var snapshotEntity = _mapper.Map<Snapshot>(snapshotCreateRequestDto);

            var createdSnapshot = await _snapshotService.CreateAsync(snapshotEntity);

            var response = _mapper.Map<SnapshotResponseDto>(createdSnapshot);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
    }
}
