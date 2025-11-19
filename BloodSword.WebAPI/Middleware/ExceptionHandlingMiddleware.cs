using BloodSword.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Serilog; // Трябва ни логърът
using System.Net;

namespace BloodSword.WebAPI.Middleware
{
    // Middleware-ът се нуждае от RequestDelegate за да извика следващия компонент в pipeline-а
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // InvokeAsync е методът, който ASP.NET Core извиква
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Извиква следващия middleware или контролера
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Улавяме всякаква грешка, която не е била хваната от контролера
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // 1. Определяме HTTP статус кода и логваме
            HttpStatusCode statusCode;
            string title;
            string detail = exception.Message;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound; // 404
                    title = "Resource Not Found";
                    break;
                case ArgumentException argumentException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Invalid Input";
                    break;
                case InvalidOperationException invalidOperationException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Invalid Operation";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError; // 500
                    title = "Server Error";
                    // Логваме 500 грешките като Fatal, за да ги видим веднага!
                    Log.Fatal(exception, "Unhandled error during request execution.");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            // 2. Връщаме стандартизиран ProblemDetails JSON (по HTTP стандарта RFC 7807)
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