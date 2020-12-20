using System;
using Hydra.Bff.Orders.Services;
using Hydra.WebAPI.Core.DelegatingHandlers;
using Hydra.WebAPI.Core.Extensions;
using Hydra.WebAPI.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Hydra.Bff.Orders.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            //Registering DI for Services
            services.AddHttpClient<ICatalogService, CatalogService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AllowSelfSignedCertificate()
                    .AddPolicyHandler(PollyExtensions.WaitAndRetry())
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IBasketService, BasketService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AllowSelfSignedCertificate()
                    .AddPolicyHandler(PollyExtensions.WaitAndRetry())
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IVoucherService, VoucherService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AllowSelfSignedCertificate()
                    .AddPolicyHandler(PollyExtensions.WaitAndRetry())
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICustomerService, CustomerService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AllowSelfSignedCertificate()
                    .AddPolicyHandler(PollyExtensions.WaitAndRetry())
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IOrderService, OrderService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AddPolicyHandler(PollyExtensions.WaitAndRetry())
                    .AllowSelfSignedCertificate()
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        }
    }
}