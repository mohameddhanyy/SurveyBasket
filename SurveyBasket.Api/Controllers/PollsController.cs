
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

public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _poll = pollService;


    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await _poll.GetAsync(id);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _poll.GetAllAsync();
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result?.Error);
    }


    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request)
    {
        var poll = await _poll.AddAsync(request);
        var pollRespone = poll.Adapt<PollResponse>();
        return CreatedAtAction(nameof(Get), new { id = poll?.Id }, pollRespone);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request)
    {
        var result = await _poll.UpdateAsync(id, request);
        return result.IsSuccess
            ? NoContent()
            : NotFound(result?.Error);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var poll = await _poll.DeleteAsync(id);
        return poll.IsSuccess
            ? NoContent()
            : NotFound(poll?.Error);
    }


    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> UpdateTogglePublish([FromRoute] int id)
    {
        var poll = await _poll.TogglePublishStatusAsync(id);
        return poll.IsSuccess
            ? NoContent()
            : NotFound(poll?.Error);
    }

}

