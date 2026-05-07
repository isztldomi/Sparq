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
            Console.WriteLine("=== SNAPSHOT CREATE START ===");

            // 1. REQUEST
            Console.WriteLine($"QuizId: {snapshotCreateRequestDto?.QuizId}");
            Console.WriteLine($"Title: {snapshotCreateRequestDto?.Title}");
            Console.WriteLine($"TimeLimit: {snapshotCreateRequestDto?.TimeLimit}");
            Console.WriteLine($"PinCode: {snapshotCreateRequestDto?.PinCode}");

            Console.WriteLine($"Questions count: {snapshotCreateRequestDto?.Questions?.Count}");

            if (snapshotCreateRequestDto?.Questions != null)
            {
                int qi = 0;

                foreach (var q in snapshotCreateRequestDto.Questions)
                {
                    Console.WriteLine($"--- QUESTION {qi} ---");
                    Console.WriteLine($"Title: {q.Title}");
                    Console.WriteLine($"Text: {q.Text}");
                    Console.WriteLine($"MediaId: {q.MediaId}");
                    Console.WriteLine($"TimeLimit: {q.TimeLimit}");
                    Console.WriteLine($"Point: {q.Point}");
                    Console.WriteLine($"Answers count: {q.Answers?.Count}");

                    if (q.Answers != null)
                    {
                        int ai = 0;

                        foreach (var a in q.Answers)
                        {
                            Console.WriteLine($"   Answer {ai}: Text={a.Text}, IsCorrect={a.IsCorrect}");
                            ai++;
                        }
                    }

                    qi++;
                }
            }

            // 2. USER
            Console.WriteLine("Loading user...");
            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
            {
                Console.WriteLine("USER NULL → Unauthorized");
                return Unauthorized();
            }

            Console.WriteLine($"User OK: {user.Id}");

            // 3. QUIZ
            Console.WriteLine($"Loading quiz: {snapshotCreateRequestDto.QuizId}");

            var quiz = await _quizService.GetByIdAsync(snapshotCreateRequestDto.QuizId);

            if (quiz == null)
            {
                Console.WriteLine("QUIZ NOT FOUND");
                return NotFound("Quiz not found.");
            }

            Console.WriteLine($"Quiz OK: Id={quiz.Id}, OwnerId={quiz.OwnerId}");

            // 4. OWNERSHIP
            if (quiz.OwnerId != user.Id)
            {
                Console.WriteLine("FORBIDDEN: not owner");
                return Forbid();
            }

            Console.WriteLine("Ownership OK");

            // 5. MAPPING
            Console.WriteLine("Mapping DTO → Entity");

            var snapshotEntity = _mapper.Map<Snapshot>(snapshotCreateRequestDto);

            if (snapshotEntity == null)
            {
                Console.WriteLine("MAPPING FAILED → NULL ENTITY");
                return StatusCode(500);
            }

            Console.WriteLine($"Mapped Questions: {snapshotEntity.Questions?.Count}");

            if (snapshotEntity.Questions != null)
            {
                int qi = 0;

                foreach (var q in snapshotEntity.Questions)
                {
                    Console.WriteLine($"[MAPPED Q {qi}] Title={q.Title}, Answers={q.Answers?.Count}");
                    qi++;
                }
            }

            // 6. CREATE
            Console.WriteLine("Calling CreateAsync...");

            var createdSnapshot = await _snapshotService.CreateAsync(snapshotEntity);

            if (createdSnapshot == null)
            {
                Console.WriteLine("CREATE FAILED → NULL");
                return StatusCode(500);
            }

            Console.WriteLine($"Created Snapshot ID: {createdSnapshot.Id}");

            // 7. RESPONSE
            Console.WriteLine("Mapping response DTO...");

            var response = _mapper.Map<SnapshotResponseDto>(createdSnapshot);

            Console.WriteLine($"Response ready: {response.Id}");

            Console.WriteLine("=== SNAPSHOT CREATE END SUCCESS ===");

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
    }
}
