using Confluent.Kafka;
using FlightDBWriter.Consumers;
using FlightDBWriter.Database;
using FlightDBWriter.Handlers;
using FlightDBWriter.HealthChecks;
using KafkaLib.Consumer;
using KafkaLib.Interfaces;
using KafkaLib.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace FlightDBWriter;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("init main");
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IFlightsRepository, FlightsRepository>();

            var clientConfig = new ClientConfig()
            {
                BootstrapServers = builder.Configuration["Kafka:ClientConfigs:BootstrapServers"]
            };
            var consumerConfig = new ConsumerConfig(clientConfig)
            {
                GroupId = "SourceApp",
                EnableAutoCommit = true,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000
            };

            builder.Services.AddSingleton(consumerConfig);
            builder.Services.AddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));
            builder.Services.AddScoped<IKafkaHandler<string, RegisterFlight>, RegisterFlightHandler>();
            //builder.Services.AddHostedService<RegisterFlightConsumer>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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