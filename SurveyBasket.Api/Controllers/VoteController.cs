using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.DTOs.Vote;
using SurveyBasket.Api.Extensions;
using SurveyBasket.Api.Presistance.Models;
using System.Security.Claims;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/vote")]
    [ApiController]
    [Authorize]
    public class VoteController(IQuestionService questionService, IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteService _voteService = voteService;

        [HttpGet("")]
        public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var questions = await _questionService.GetAvailableAsync(pollId, userId!, cancellationToken);

            if (questions.IsSuccess)
                return Ok(questions.Value);

            return questions.Error.Equals(VoteError.DuplicatedVote)
                ? Conflict(VoteError.DuplicatedVote)
                : NotFound(PollErrors.NoPollFound);
        }

        [HttpPost("")]
        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request)
        {
            var userId = User.GetUserId();
            var result = await _voteService.AddAsync(pollId, userId!, request);
            if (result.IsSuccess)
                return Created();


            return result.Error.Equals(VoteError.DuplicatedVote)
                ? Conflict(VoteError.DuplicatedVote)
                : NotFound(VoteError.InCorrectQuestions);

        }

    }
}
