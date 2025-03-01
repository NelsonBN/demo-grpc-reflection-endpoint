using ProtoBuf;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts.ProtobufNet;

[ServiceContract]
public interface IGreeterService
{
    [OperationContract]
    Task<MessageResponse> GetMessage(MessageRequest request, CallContext context = default);

    [OperationContract]
    Task GenerateMessage(MessageRequest request, CallContext context = default);
}


[ProtoContract(SkipConstructor = true)]
public record MessageRequest(
    [property: ProtoMember(1)] string Message);

[ProtoContract(SkipConstructor = true)]
public record MessageResponse(
    [property: ProtoMember(1)] string Message);