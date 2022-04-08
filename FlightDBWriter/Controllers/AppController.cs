using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightDBWriter.Controllers;

public class AppController : ControllerBase
{
    [HttpGet]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation("Get Welcome Message", "This is a dummy endpoint")]
    public IActionResult Get()
    {
        return Ok("Welcome to Kafka Consumer Application.");
    }

}