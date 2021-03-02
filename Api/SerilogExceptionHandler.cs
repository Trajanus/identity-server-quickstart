using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Api
{
    public class SerilogExceptionHandler
    {
        private readonly RequestDelegate _next;

        public SerilogExceptionHandler(RequestDelegate next)
        {
            _next = next;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"c:\\logs\\identity-server-quickstart-api\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Unhandled exception in the http pipeline.");
            }
        }
    }

    public static class SerilogExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseSerilogExceptionHandler(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogExceptionHandler>();
        }
    }
}