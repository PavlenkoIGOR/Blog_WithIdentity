using MainBlog.BL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Xml;

namespace MainBlog
{
    public class MyExceptionHandlerMiddleware
    {
        #region 
        private readonly  RequestDelegate _next;
        private readonly ILogger<MyExceptionHandlerMiddleware> _logger;
        IWebHostEnvironment _env;

        public MyExceptionHandlerMiddleware(RequestDelegate next, ILogger<MyExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


                var response = JsonSerializer.Serialize(new { error = ex.Message });
                await WriteActions.CreateLogFolder_File(_env, "GlobalErrors", $"{ex.Message}");
                await context.Response.WriteAsync(response);
            }
        }
        #endregion
    }
}
