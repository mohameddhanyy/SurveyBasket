using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SurveyBasket.Api.Presistance.Models;
using System.Threading.Tasks;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;


        [HttpGet("")]
        [OutputCache(PolicyName ="Polls")]
        public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellation)
        {
            var result = await _questionService.GetAllAsync(pollId, cancellation);
            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellation)
        {
            var result = await _questionService.GetAsync(pollId, id, cancellation);
            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellation)
        {
            var result = await _questionService.AddAsync(pollId, request, cancellation);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);

            return result.Error.Equals(QuestionError.NoQuestionFound)
                ? NotFound(result.Error)
                : Conflict(result.Error);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int pollId,[FromRoute] int id ,[FromBody] QuestionRequest request, CancellationToken cancellation)
        {
            var result = await _questionService.UpdateAsync(pollId,id, request, cancellation);

            if (result.IsSuccess)
                return NoContent();

            return result.Error.Equals(QuestionError.NoQuestionFound)
                ? NotFound(result.Error)
                : Conflict(result.Error);
        }


        [HttpPut("{id}/toggleStatus")]
        public async Task<IActionResult> UpdateToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellation)
        {
            var question = await _questionService.ToggleStatusAsync(pollId,id,cancellation);
            return question.IsSuccess
                ? NoContent()
                : NotFound(question?.Error);
        }

    }
}
