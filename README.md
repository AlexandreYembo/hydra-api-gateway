# hydra-api-gateway
This repository contains few projects used for orchestration of process, such Orders, etc




### GRPC packages:
- Google.Protobuf -> to understand the Protobuf.

- Grpc.Net.ClientFactory -> Access the external service remotely.

- Grpc.Tools -> used for bind and serialization of the objects.

### GRPC configuration
In the csProject there is a specific key:
```cs
<ItemGroup>
    <Protobuf Include="Protos\basket.proto" GrpcServices="Client" />
</ItemGroup>
```

#### gRPC support
gRPC does not support to run in IIS, so you should run as SelfHosting.