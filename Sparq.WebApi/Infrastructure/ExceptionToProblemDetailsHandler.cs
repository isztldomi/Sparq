using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Exceptions;

namespace Sparq.WebApi.Infrastructure
{
    /// <summary>
    /// Global exception handler that converts exceptions into RFC 7807 ProblemDetails responses.
    /// </summary>
    public class ExceptionToProblemDetailsHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        /// <summary>
        /// Creates a new instance of <see cref="ExceptionToProblemDetailsHandler"/>.
        /// </summary>
        /// <param name="problemDetailsService">Service used to write ProblemDetails responses.</param>
        public ExceptionToProblemDetailsHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        /// <summary>
        /// Attempts to handle an exception and convert it into a ProblemDetails response.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <param name="exception">The thrown exception.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// True if the exception was handled and a response was written; otherwise false.
        /// </returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            return exception switch
            {
                EntityNotFoundException => await CreateProblemDetails(httpContext, exception, StatusCodes.Status404NotFound),
                SaveFailedException => await CreateProblemDetails(httpContext, exception, StatusCodes.Status409Conflict),
                ArgumentOutOfRangeException => await CreateProblemDetails(httpContext, exception, StatusCodes.Status400BadRequest),
                ArgumentNullException => await CreateProblemDetails(httpContext, exception, StatusCodes.Status400BadRequest),
                ArgumentException => await CreateProblemDetails(httpContext, exception, StatusCodes.Status409Conflict),
                InvalidDataException => await CreateProblemDetails(httpContext, exception, StatusCodes.Status409Conflict),
                InvalidOperationException => await CreateProblemDetails(httpContext, exception, StatusCodes.Status409Conflict),
                _ => false
            };
        }

        /// <summary>
        /// Creates and writes a ProblemDetails response to the HTTP context.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="statusCode">HTTP status code to return.</param>
        /// <returns>True if the response was successfully written.</returns>
        private async Task<bool> CreateProblemDetails(
            HttpContext httpContext,
            Exception exception,
            int statusCode)
        {
            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred",
                Type = exception.GetType().Name,
                Detail = exception.Message
            };

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                Exception = exception,
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
        }
    }
}