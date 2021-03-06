using Hydra.Bff.Orders.Setup;
using Hydra.WebAPI.Core.Identity;
using Hydra.WebAPI.Core.Setups;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hydra.Bff.Orders
{
    public class Startup
    {
        public Startup(IHostEnvironment hostEnvironment)
        {
           Configuration = HostEnvironmentConfiguration.AddHostEnvironment(hostEnvironment);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfiguration(Configuration);
            services.AddJwtConfiguration(Configuration);
            services.AddSwaggerConfig();
            services.RegisterServices();
            services.AddMessageBusConfiguration(Configuration);
            services.RegisterGrpcService(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfig();
            app.UseApiConfiguration(env);
        }
    }
}
