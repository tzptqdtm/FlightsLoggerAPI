using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaLib.Interfaces;
using KafkaLib.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KafkaLib.Consumer
{
    public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue> where TValue : class
    {
        private readonly ConsumerConfig _config;
        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IKafkaHandler<TKey, TValue> _handler;
        private IConsumer<TKey, TValue> _consumer;
        private string _topic;

        public KafkaConsumer(ConsumerConfig config, IServiceScopeFactory serviceScopeFactory, ILogger<KafkaConsumer<TKey, TValue>> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(string topic, CancellationToken stoppingToken)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be null or whitespace.", nameof(topic));

            using var scope = _serviceScopeFactory.CreateScope();
            _handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<TKey, TValue>>();
            _consumer = new ConsumerBuilder<TKey, TValue>(_config)
                .SetValueDeserializer(new KafkaDeserializer<TValue>())
                .Build();
            _topic = topic;

            await StartConsumerLoop(stoppingToken);
        }

        public void Close()
        {
            _consumer?.Close();
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }

        private async Task StartConsumerLoop(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(cancellationToken);
                    if (result != null)
                    {
                        await _handler.HandleAsync(result.Message.Key, result.Message.Value);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    _logger.LogError(e, "An error occurred while consuming the message");
                    if (e.Error.IsFatal)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An unexpected error occurred while consuming the message");
                    break;
                }
            }
        }
    }
}