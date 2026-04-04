using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.QuizDto;
using System.Security.Claims;

namespace Sparq.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IMapper _mapper;

        public QuizController(IQuizService quizService, IMapper mapper)
        {
            _quizService = quizService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] QuizCreateRequestDto quizCreateRequestDto)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();

            var quiz = _mapper.Map<Quiz>(quizCreateRequestDto);

            Console.WriteLine("Snapshots in mapped object: " + (quiz.Snapshots?.Count ?? 0));
            quiz.OwnerId = userId!;

            foreach (var snapshot in quiz.Snapshots!)
            {
                snapshot.CreatedAt = DateTime.UtcNow;
            }

            var savedQuiz = await _quizService.CreateAsync(quiz);

            var quizResponseDto = _mapper.Map<QuizResponseDto>(savedQuiz);

            return CreatedAtAction(nameof(GetById), new { id = quizResponseDto.Id }, quizResponseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quizEntity = await _quizService.GetByIdAsync(id);

            if (quizEntity == null)
                return NotFound();

            var response = _mapper.Map<QuizResponseDto>(quizEntity);

            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quizEntities = await _quizService.GetAllAsync();
            var response = _mapper.Map<List<QuizResponseDto>>(quizEntities);
            return Ok(response);
        }
    }
}