using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController(IConfiguration configuration) : ControllerBase
    {
        [HttpGet("{key}")]
        public IActionResult GetConfigValue(string key)
        {
            // This will look through all configuration providers, including Azure Key Vault,
            // to find a matching key/secret name.
            var value = configuration[key];

            if (string.IsNullOrEmpty(value))
            {
                return NotFound(new { Message = $"The key '{key}' was not found in the configuration or Key Vault." });
            }

            return Ok(new { Key = key, Value = value });
        }
    }
}