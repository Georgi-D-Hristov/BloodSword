using BloodSword.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace BloodSword.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            string title;
            string detail = exception.Message;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    title = "Resource Not Found";
                    break;
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Invalid Input";
                    break;
                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Invalid Operation";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    title = "Server Error";
                    Log.Fatal(exception, "Unhandled error during request execution.");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            return context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
