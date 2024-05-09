using StackExchange.Redis;
using System.Net;
using System.Text;

namespace CodeBlog.API.Middlwares
{
    public class GlobalExceptionHandalingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandalingMiddleware> _logger;

        public GlobalExceptionHandalingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandalingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext http)
        {
            try
            {
                await _next(http);
            }
            catch(RedisConnectionException ex)
            {
                http.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes("Apperentrly redis is not connected"));
                http.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                _logger.LogError(ex, ex.Message);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                http.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
