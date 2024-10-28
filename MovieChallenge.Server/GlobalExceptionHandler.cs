using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieChallenge.BLL.Exceptions;

namespace MovieChallenge.Server
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ExtendedProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error",
                UiFriendlyError = exception is UiFriendlyException ? exception.Message : ""
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }

    //todo move this into its own file
    public class ExtendedProblemDetails : ProblemDetails
    {
        public string UiFriendlyError { get; set; }
    }
}
