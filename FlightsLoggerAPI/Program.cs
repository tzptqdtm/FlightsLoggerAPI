using Confluent.Kafka;
using KafkaLib.Interfaces;
using KafkaLib.Producer;

var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder.Configuration);
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

void ConfigureConfiguration(ConfigurationManager configuration)
{
    //
}

void ConfigureServices(IServiceCollection services)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    // Add services to the container.
    services.AddControllers();

    var producerConfig = new ProducerConfig(new ClientConfig
    {
        BootstrapServers = builder.Configuration["Kafka:ClientConfigs:BootstrapServers"]
    });

    services.AddSingleton(producerConfig);
    services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));
}