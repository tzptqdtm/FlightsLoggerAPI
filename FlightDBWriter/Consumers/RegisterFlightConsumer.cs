using KafkaLib.Constants;
using KafkaLib.Interfaces;
using KafkaLib.Messages;

namespace FlightDBWriter.Consumers;

public class RegisterFlightConsumer : BackgroundService
{
	private readonly IKafkaConsumer<string, RegisterFlight> _consumer;
    private readonly ILogger<RegisterFlightConsumer> _logger;
    public RegisterFlightConsumer(IKafkaConsumer<string, RegisterFlight> kafkaConsumer, ILogger<RegisterFlightConsumer> logger)
    {
        _consumer = kafkaConsumer;
        _logger = logger;
    }
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _consumer.Consume(KafkaTopics.RegisterFlight, stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while consuming the message");
        }
    }


}