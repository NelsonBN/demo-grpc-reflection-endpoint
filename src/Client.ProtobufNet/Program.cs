using Contracts.ProtobufNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.ClientFactory;
using System;
using System.Threading;
using System.Threading.Tasks;


var builder = Host.CreateApplicationBuilder(args);


builder.Services
    .AddCodeFirstGrpcClient<IGreeterService>((sp, options)
        => options.Address = new Uri(sp.GetRequiredService<IConfiguration>()["ServiceEndpoint"]!));

builder.Services.AddHostedService<WorkerGetMessage>();
builder.Services.AddHostedService<WorkerGenerateMessage>();


var app = builder.Build();


await app.RunAsync();


public class WorkerGetMessage(
    ILogger<WorkerGetMessage> logger,
    IGreeterService client) : BackgroundService
{
    private readonly ILogger<WorkerGetMessage> _logger = logger;
    private readonly IGreeterService _client = client;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = await _client.GetMessage(new MessageRequest("Ping"));

                _logger.LogInformation("[Client][GetGenerateMessage][{Time}] - '{Message}'", DateTimeOffset.UtcNow, response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Client][GetGenerateMessage] Error occurred");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}


public class WorkerGenerateMessage(
    ILogger<WorkerGenerateMessage> logger,
    IGreeterService client) : BackgroundService
{
    private readonly ILogger<WorkerGenerateMessage> _logger = logger;
    private readonly IGreeterService _client = client;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _client.GenerateMessage(new("Ping"));

                _logger.LogInformation("[Client][GenerateMessage][{Time}]", DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Client][GenerateMessage] Error occurred");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
