
using Core.Models;
using Core.ServiceContracts;
using FluentValidation;
using Mapster;
using MapsterMapper;
using SurveyBasket.Api.DTOs.Requests;
using SurveyBasket.Api.DTOs.Responses;
namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService, IMapper mapper) : ControllerBase
{
    private readonly IPollService _poll = pollService;
    private readonly IMapper _mapper = mapper;


    [HttpGet("")]
    public IActionResult GetAll()
    {
        var polls = _poll.GetAll();
        if (polls is null) NotFound();
        var pollResponse = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(pollResponse);
    }


    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _poll.Get(id);
        if (poll is null) NotFound();
        var pollResponse = poll.Adapt<PollResponse>();
        return Ok(pollResponse);
    }


    [HttpPost("")]
    public IActionResult Add([FromBody] PollRequest request)
    {
        var poll = _poll.Add(request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new { id = poll?.Id }, poll);
    }


    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] PollRequest request)
    {
        var poll = _poll.Update(id, request.Adapt<Poll>());
        if (!poll) return NotFound();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var poll = _poll.Delete(id);
        if (!poll) return NotFound();
        return NoContent();
    }
}

