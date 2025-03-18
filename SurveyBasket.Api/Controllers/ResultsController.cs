namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultsController(IResultService resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;

        [HttpGet("row-data")]
        public async Task<IActionResult> PollVotes([FromRoute] int pollId)
        {
            var result = await _resultService.GetPollVotesAsync(pollId);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("votes-per-day")]
        public async Task<IActionResult> VotesPerDay([FromRoute] int pollId)
        {
            var result = await _resultService.GetVotesPerDayAsync(pollId);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

    }
}
