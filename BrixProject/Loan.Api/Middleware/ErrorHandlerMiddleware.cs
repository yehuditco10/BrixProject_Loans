using Loan.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Loan.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode code = HttpStatusCode.BadRequest;
            string result = JsonConvert.SerializeObject(new { error = ex.Message });

            if (ex is DataNotFoundException)
                code = HttpStatusCode.NotFound;
            else
            code = HttpStatusCode.InternalServerError;

            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
