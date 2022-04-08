using Confluent.Kafka;
using FlightDBWriter.Consumers;
using FlightDBWriter.Database;
using FlightDBWriter.Handlers;
using KafkaLib.Consumer;
using KafkaLib.Interfaces;
using KafkaLib.Messages;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);


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

app.Run();

void ConfigureServices(IServiceCollection services)
{
    // Add services to the container.

    services.AddControllers();

    services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    services.AddScoped<IFlightsRepository, FlightsRepository>();

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

    services.AddSingleton(consumerConfig);
    services.AddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));
    services.AddScoped<IKafkaHandler<string, RegisterFlight>, RegisterFlightHandler>();
    services.AddHostedService<RegisterFlightConsumer>();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();


}