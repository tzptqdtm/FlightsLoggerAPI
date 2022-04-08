using KafkaLib.Constants;
using KafkaLib.Interfaces;
using KafkaLib.Messages;
using Microsoft.AspNetCore.Mvc;

namespace FlightsLoggerAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IKafkaProducer<string, RegisterFlight> _kafkaProducer;
    public FlightsController(IKafkaProducer<string, RegisterFlight> kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }


    [HttpPost]
    [Route("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    
    public async Task<IActionResult> ProduceMessage(RegisterFlight request)
    {
        await _kafkaProducer.ProduceAsync(KafkaTopics.RegisterFlight, null, request);

        return Ok("Flight Registration In Progress");
    }
}