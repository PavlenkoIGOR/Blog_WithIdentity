﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;


namespace MainBlog
{
    public class MyExceptionHandlerMiddleware
    {
        #region оригинал
        /*Оригинал*/
        private readonly  RequestDelegate _next;
        private readonly ILogger<MyExceptionHandlerMiddleware> _logger;

        public MyExceptionHandlerMiddleware(RequestDelegate next, ILogger<MyExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
                await context.Response.WriteAsync(response);
            }
        }
        #endregion
    }
}