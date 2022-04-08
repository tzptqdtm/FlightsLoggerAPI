using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightDBWriter.Controllers;

[Route("[controller]")]
public class AppController : ControllerBase
{

    //Эндпоинт на всякий случай. В данный момент, ничего не делает. 

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation("Get Welcome Message", "This is a dummy endpoint")]
    public IActionResult Get()
    {
        return Ok("Welcome to Kafka Consumer Application.");
    }

}