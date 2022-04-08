using System.Net;
using KafkaLib.Constants;
using KafkaLib.Interfaces;
using KafkaLib.Messages;

namespace FlightDBWriter.Consumers;

public class RegisterFlightConsumer : BackgroundService
{
	private readonly IKafkaConsumer<string, RegisterFlight> _consumer;
    public RegisterFlightConsumer(IKafkaConsumer<string, RegisterFlight> kafkaConsumer)
    {
        _consumer = kafkaConsumer;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _consumer.Consume(KafkaTopics.RegisterFlight, stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{(int)HttpStatusCode.InternalServerError} ConsumeFailedOnTopic - {KafkaTopics.RegisterFlight}, {ex}");
        }
    }


}