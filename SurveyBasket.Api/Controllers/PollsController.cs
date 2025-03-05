
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.DTOs.Polls;
using SurveyBasket.Api.Presistance.Models;
using SurveyBasket.Api.ServiceContracts;
using System.Threading.Tasks;
namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class PollsController(IPollService pollService, IMapper mapper) : ControllerBase
{
    private readonly IPollService _poll = pollService;
    private readonly IMapper _mapper = mapper;


    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _poll.GetAllAsync();
        if (polls is null) NotFound();
        var pollResponse = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(pollResponse);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var poll = await _poll.GetAsync(id);
        if (poll is null) NotFound();
        var pollResponse = poll.Adapt<PollResponse>();
        return Ok(pollResponse);
    }


    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request)
    {
        var poll = await _poll.AddAsync(request.Adapt<Poll>());
        var pollRespone = poll.Adapt<PollResponse>();
        return CreatedAtAction(nameof(Get), new { id = poll?.Id }, pollRespone);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request)
    {
        var poll = await _poll.UpdateAsync(id, request.Adapt<Poll>());
        if (!poll) return NotFound();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var poll = await _poll.DeleteAsync(id);
        if (!poll) return NotFound();
        return NoContent();
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> UpdateTogglePublish([FromRoute] int id)
    {
        var poll = await _poll.TogglePublishStatusAsync(id);
        if (!poll) return NotFound();
        return NoContent();
    }

}

