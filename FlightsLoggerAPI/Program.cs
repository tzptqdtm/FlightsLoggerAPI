using Confluent.Kafka;
using FlightsLoggerAPI.HealthChecks;
using KafkaLib.Interfaces;
using KafkaLib.Producer;
using NLog;
using NLog.Web;

namespace FlightsLoggerAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("init main");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Add services to the container.
            builder.Services.AddControllers();

            var producerConfig = new ProducerConfig(new ClientConfig
            {
                BootstrapServers = builder.Configuration["Kafka:ClientConfigs:BootstrapServers"]
            });

            builder.Services.AddSingleton(producerConfig);
            builder.Services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));
            builder.Services.AddHealthChecks().AddCheck<HealthCheck>("Test");
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/healthz");

            app.Run();
        }
        catch (Exception exception)
        {
            // NLog: catch setup errors
            logger.Error(exception, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }

    }
}