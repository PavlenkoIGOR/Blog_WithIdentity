using MainBlog.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace MainBlog.Data
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next/*, ILogger logger*/)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"An error occurred: {ex}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(response);
            }
        }
    }
}


