using FlightDBWriter.Database;
using KafkaLib.Constants;
using KafkaLib.Interfaces;
using KafkaLib.Messages;

namespace FlightDBWriter.Handlers;

public class RegisterFlightHandler : IKafkaHandler<string, RegisterFlight>
{
    private readonly IFlightsRepository _flightsRepository;
    private readonly ILogger<RegisterFlightHandler> _logger;
    public RegisterFlightHandler(IFlightsRepository flightsRepository, ILogger<RegisterFlightHandler> logger)
    {
        _flightsRepository = flightsRepository;
        _logger = logger;
    }
    public async Task HandleAsync(string key, RegisterFlight value)
    {
        try
        {
            var flight = new Flight { DepartureDate = value.DepartureDate, FlightNumber = value.FlightNumber };
            
            var isSuccess = await _flightsRepository.Add(flight);
            if (!isSuccess)
            {
                _logger.LogError("An error occurred while adding the flight to the database");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while handling the message");
        }

    }
}