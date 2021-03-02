using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                });
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine("Application is shutting down.");
                Log.CloseAndFlush();
            }, true);

            app.UseSerilogExceptionHandler();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
