using System;
using Hydra.Bff.Orders.Services.gRPC;
using Hydra.gRPC.Core.Config;
using Hydra.WebAPI.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BasketGrpc = Hydra.Basket.API.Services.gRPC.Basket;

namespace Hydra.Bff.Orders.Setup
{
    public static class GrpcServicesConfig
    {
        public static void RegisterGrpcService(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterInterceptor();

            services.AddScoped<IBasketGrpcService, BasketGrpcService>();
            
            //Client Factory, it will consume the service remotely.
            services.AddGrpcClient<BasketGrpc.BasketClient>(options => 
            {
                options.Address = new Uri(configuration["BasketUrl"]);
            }).AddgGRPCInterceptor()
              .AllowSelfSignedCertificate();
        }
    }
}