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

##### Important:
If you are developing your project gRPC under another Operation system such MacOS, it will need to do few adjusts on the project, because I have stroggled few hours to figured out how to fix the error related to HTTP 2.0 communication error. You can add this code on the ```GrpcServicesConfig.cs``` file:
```cs
  if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                        // The following statement allows you to call insecure services. To be used only in development environments.
                        AppContext.SetSwitch(
                            "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                        options.Address = new Uri("http://localhost:45055");
                }
                else
                options.Address = new Uri(configuration["BasketUrl"]);
```
###### This should not be used in production deployment, only for development and test propose. Also including this line will break others request such HttpClient, so beware that using this solution is only to call gRPC functions, but in case that you need to test Rest API rather than gRPC, you have to get rid of this solution.
