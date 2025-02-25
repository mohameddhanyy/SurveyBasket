
using Core.Models;
using Core.ServiceContracts;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _poll = pollService; 

    [HttpGet("")]
    public IActionResult GetAll()
    {
        return Ok(_poll.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(_poll.Get(id));
    }

    [HttpPost("")]
    public IActionResult Add(Poll request)
    {
        var newPoll = _poll.Add(request);
        return CreatedAtAction(nameof(Get), new { id = newPoll?.Id }, newPoll);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Poll poll)
    {
        var result = _poll.Update(id, poll);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _poll.Delete(id);
        if (!result) return NotFound();
        return NoContent();
    }
}

