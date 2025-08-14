using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace WebApplication1.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly Serilog.ILogger _logger;

        public GlobalExceptionFilter()
        {
            _logger = Serilog.Log.ForContext<GlobalExceptionFilter>();

        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            _logger.Error(exception, "Unhandled exception in controller action");

            var response = new
            {
                StatusCode = 500,
                Message = "An unexpected error occurred.",
                Detailed = exception.Message 
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}
