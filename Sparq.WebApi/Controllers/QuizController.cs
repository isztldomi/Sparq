using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.Page;
using Sparq.Shared.Models.QuizDto;
using System.Security.Claims;

namespace Sparq.WebApi.Controllers
{
    /// <summary>
    /// Handles quiz-related operations such as creation and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuizService _quizService;
        private readonly IUsersService _usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance for DTO-entity conversions.</param>
        /// <param name="quizService">Service handling quiz business logic.</param>
        public QuizController(IMapper mapper, IQuizService quizService, IUsersService usersService)
        {
            _mapper = mapper;
            _quizService = quizService;
            _usersService = usersService;
        }

        /// <summary>
        /// Quiz by Id
        /// </summary>
        /// <param name="id">The quiz identifier.</param>
        /// <returns>The quiz if found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuizResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var quizEntity = await _quizService.GetByIdAsync(id);

            if (quizEntity == null)
                return NotFound();

            var response = _mapper.Map<QuizResponseDto>(quizEntity);

            return Ok(response);
        }

        /// <summary>
        /// All quizzes
        /// </summary>
        /// <returns>List of quizzes.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<QuizResponseDto>))]
        public async Task<IActionResult> GetAll()
        {
            var quizEntities = await _quizService.GetAllAsync();
            var response = _mapper.Map<List<QuizResponseDto>>(quizEntities);
            return Ok(response);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="quizCreateRequestDto">User creation request data.</param>
        /// <returns>The created quiz.</returns>
        /// <remarks>
        /// The owner of the quiz is automatically assigned from the authenticated user context.
        /// Each snapshot is initialized with a creation timestamp.
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuizResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] QuizCreateRequestDto quizCreateRequestDto)
        {

            var quiz = _mapper.Map<Quiz>(quizCreateRequestDto);

            Console.WriteLine("Snapshots in mapped object: " + (quiz.Snapshots?.Count ?? 0));


            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound();
            }

            //foreach (var snapshot in quiz.Snapshots!)
            //{
            //    snapshot.CreatedAt = DateTime.UtcNow;
            //}

            quiz.OwnerId = user.Id;

            var savedQuiz = await _quizService.CreateAsync(quiz);

            var quizResponseDto = _mapper.Map<QuizResponseDto>(savedQuiz);

            return CreatedAtAction(nameof(GetById), new { id = quizResponseDto.Id }, quizResponseDto);
        }

        /// <summary>
        /// Current user's quizzes (paged)
        /// </summary>
        [HttpGet("mine")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<MyQuizListDto>))]
        public async Task<IActionResult> GetMyQuizzes(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Invalid paging parameters.");

            var user = await _usersService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var (items, totalCount) = await _quizService.GetByUserPagedAsync(user.Id, page, pageSize);

            var mapped = _mapper.Map<List<MyQuizListDto>>(items);

            var result = new PagedResult<MyQuizListDto>
            {
                Items = mapped,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }
    }
}