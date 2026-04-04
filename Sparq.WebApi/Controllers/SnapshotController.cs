using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sparq.Shared.Models.SnapshotDto;
using Sparq.DataAccess.Services;
using Sparq.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;

namespace Sparq.WebApi.Controllers
{
    /// <summary>
    /// API controller responsible for managing snapshots.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SnapshotController : ControllerBase
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotController"/> class.
        /// </summary>
        /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
        /// <param name="snapshotService">Service for snapshot business logic.</param>
        public SnapshotController(IMapper mapper, ISnapshotService snapshotService)
        {
            _mapper = mapper;
            _snapshotService = snapshotService;
        }

        /// <summary>
        /// Retrieves all snapshots from the system.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Snapshot>>> GetAll()
        {
            var snapshots = await _snapshotService.GetAllAsync();
            var snapshotResponseDto = _mapper.Map<IReadOnlyCollection<SnapshotResponseDto>>(snapshots);
            return Ok(snapshotResponseDto);
        }

        /// <summary>
        /// Retrieves a snapshot by its unique identifier.
        /// </summary>
        /// <param name="id">The snapshot ID.</param>
        /// <returns>The snapshot if found; otherwise 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Snapshot>> GetById(int id)
        {
            var snapshot = await _snapshotService.GetByIdAsync(id);
            if (snapshot == null)
                return NotFound();

            var snapshotResponseDto = _mapper.Map<SnapshotResponseDto>(snapshot);
            return Ok(snapshotResponseDto);
        }

        /// <summary>
        /// Creates a new snapshot including its questions and answers.
        /// </summary>
        /// <param name="dto">The snapshot creation request data.</param>
        /// <returns>The created snapshot ID and data.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(SnapshotCreateRequestDto dto)
        {
            var snapshot = new Snapshot
            {
                QuizId = dto.QuizId,
                Title = dto.Title,
                Description = dto.Description,
                TimeLimit = dto.TimeLimit,
                SnapshotNumber = 0
            };

            foreach (var q in dto.Questions)
            {
                var question = new Question
                {
                    Title = q.Title,
                    Text = q.Text,
                    MediaUrl = q.MediaUrl,
                    Point = q.Point
                };

                foreach (var a in q.Answers)
                {
                    question.Answers.Add(new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    });
                }

                snapshot.Questions.Add(question);
            }

            var result = await _snapshotService.CreateAsync(snapshot);
            var snapshotResponseDto = _mapper.Map<SnapshotResponseDto>(result);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                snapshotResponseDto
            );
        }
    }
}