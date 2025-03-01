using Contracts.Raw.Messages;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks; // Add this namespace for reflection services

var builder = WebApplication.CreateSlimBuilder(args);


builder.Services
    .AddGrpc(options =>
        options.EnableDetailedErrors = true);
builder.Services.AddGrpcReflection(); // Add gRPC reflection service

var app = builder.Build();


app.MapGrpcService<GrpcService>();
app.MapGrpcReflectionService(); // Map the reflection service endpoint


await app.RunAsync();



public sealed class GrpcService(ILogger<GrpcService> logger) : Greeter.GreeterBase
{
    private readonly ILogger<GrpcService> _logger = logger;

    public override Task<MessageResponse> GetMessage(MessageRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[SERVER][GetMessage] '{Message}'", request.Message);

        return Task.FromResult(new MessageResponse()
        {
            Message = "Hello, World!",
        });
    }

    public override Task<Empty> GenerateMessage(MessageRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[SERVER][GenerateMessage] '{Message}'", request.Message);
        return Task.FromResult(new Empty());
    }
}
