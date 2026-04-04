using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.QuizDto;
using System.Security.Claims;

namespace Sparq.WebApi.Controllers
{
    /// <summary>
    /// API controller responsible for managing quizzes.
    /// Provides endpoints for creating, reading, updating, and deleting quizzes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizController"/> class.
        /// </summary>
        /// <param name="mapper">AutoMapper instance used for mapping between entities and DTOs.</param>
        /// <param name="quizService">Service handling quiz-related business logic.</param>
        public QuizController(IMapper mapper, IQuizService quizService)
        {
            _mapper = mapper;
            _quizService = quizService;
        }

        /// <summary>
        /// Retrieves all quizzes from the system.
        /// </summary>
        /// <returns>A collection of quiz response DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Quiz>>> GetAll()
        {
            var quizzes = await _quizService.GetAllAsync();
            var quizResponseDto = _mapper.Map<IReadOnlyCollection<QuizResponseDto>>(quizzes);

            return Ok(quizResponseDto);
        }

        /// <summary>
        /// Retrieves a specific quiz by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the quiz.</param>
        /// <returns>The requested quiz if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetById(int id)
        {
            var quiz = await _quizService.GetByIdAsync(id);

            if (quiz == null)
                return NotFound();

            var quizResponseDto = _mapper.Map<QuizResponseDto>(quiz);
            return Ok(quizResponseDto);
        }

        /// <summary>
        /// Creates a new quiz for the currently authenticated user.
        /// </summary>
        /// <param name="quizRequestDto">The quiz creation data.</param>
        /// <returns>The created quiz.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<QuizResponseDto>> Create(QuizRequestDto quizRequestDto)
        {
            var quiz = _mapper.Map<Quiz>(quizRequestDto);
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue("id");
            quiz.OwnerId = userId;
            await _quizService.CreateAsync(quiz);
            var quizResponseDto = _mapper.Map<QuizResponseDto>(quiz);
            return CreatedAtAction(
                nameof(GetById),
                new { id = quiz.Id },
                quizResponseDto
            );
        }

        /// <summary>
        /// Updates an existing quiz.
        /// </summary>
        /// <param name="id">The ID of the quiz to update.</param>
        /// <param name="quiz">The updated quiz data.</param>
        /// <returns>The updated quiz if successful; otherwise NotFound.</returns>
        [HttpPut("{id}")]
        [Authorize]
        // Csak a quiz tulajdonosa vagy admin frissítheti a quizt
        public async Task<ActionResult<Quiz>> Update(int id, [FromBody] Quiz quiz)
        {
            var updatedQuiz = await _quizService.UpdateAsync(id, quiz);

            if (updatedQuiz == null)
                return NotFound();

            var quizResponseDto = _mapper.Map<QuizResponseDto>(updatedQuiz);
            return Ok(updatedQuiz);
        }

        /// <summary>
        /// Deletes a quiz by its identifier.
        /// </summary>
        /// <param name="id">The ID of the quiz to delete.</param>
        /// <returns>No content if deletion was successful; otherwise NotFound.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _quizService.DeleteAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
