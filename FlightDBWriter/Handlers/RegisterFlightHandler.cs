using FlightDBWriter.Database;
using KafkaLib.Constants;
using KafkaLib.Interfaces;
using KafkaLib.Messages;

namespace FlightDBWriter.Handlers;

public class RegisterFlightHandler : IKafkaHandler<string, RegisterFlight>
{
    private readonly IFlightsRepository _flightsRepository;
    public RegisterFlightHandler(IFlightsRepository flightsRepository)
    {
        _flightsRepository = flightsRepository;
    }
    public async Task HandleAsync(string key, RegisterFlight value)
    {
        try
        {
            var flight = new Flight { DepartureDate = value.DepartureDate, FlightNumber = value.FlightNumber };
            
            var isSuccess = await _flightsRepository.Add(flight);
            if (!isSuccess)
            {
                Console.WriteLine($"Something went wrong.");
            }
            else
            {
                Console.WriteLine($"Flight schedule changed: \n DepartureDate: {value.DepartureDate}\n FlightNumber: {value.FlightNumber}");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

    }
}