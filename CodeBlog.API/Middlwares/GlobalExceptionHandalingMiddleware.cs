using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Net;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace CodeBlog.API.Middlwares
{
    public class GlobalExceptionHandalingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandalingMiddleware> _logger;

        public GlobalExceptionHandalingMiddleware(ILogger<GlobalExceptionHandalingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.GatewayTimeout,
                    Type = "Server error",
                    Title = "Trouble with Redis",
                    Detail = "Aperrently redis is not connected"
                };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = " Server error",
                    Title = "Server error",
                    Detail = "Unknown error"
                };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
