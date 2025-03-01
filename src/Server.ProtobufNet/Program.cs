using Contracts.ProtobufNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Server;
using System.Threading.Tasks;

var builder = WebApplication.CreateSlimBuilder(args);


builder.Services
    .AddCodeFirstGrpcReflection()
    .AddCodeFirstGrpc(options =>
        options.EnableDetailedErrors = true);

var app = builder.Build();


app.MapCodeFirstGrpcReflectionService();
app.MapGrpcService<GrpcService>();


await app.RunAsync();



public sealed class GrpcService(ILogger<GrpcService> logger) : IGreeterService
{
    private readonly ILogger<GrpcService> _logger = logger;

    public Task<MessageResponse> GetMessage(MessageRequest request, CallContext context = default)
    {
        _logger.LogInformation("[SERVER][GetMessage] '{Message}'", request.Message);

        return Task.FromResult(new MessageResponse("Hello, World!"));
    }

    public async Task GenerateMessage(MessageRequest request, CallContext context = default)
    {
        _logger.LogInformation("[SERVER][GenerateMessage] '{Message}'", request.Message);

        await Task.Delay(10);

        //        return Task.FromResult(new Empty());
    }
}
