namespace Tools.Security
{
    public class CustomSecurityMiddleware(ILogger<CustomSecurityMiddleware> logger, SecurityOptionsBuilder optionsBuilder) : IMiddleware
    {
        private readonly ILogger _logger = logger;
        private readonly SecurityOptionsBuilder _optionsBuilder = optionsBuilder;
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation("Middleware Called...");

            foreach(HeaderInfo header in _optionsBuilder.Headers)
            {
                if(!context.Request.Headers.TryGetValue(header.HeaderName, out var value) || string.IsNullOrWhiteSpace(value))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { ErrorMessage = "Invalid headers" });
                    return;
                }
                else if(header.Validator is not null && !header.Validator(value!))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { ErrorMessage = "Invalid headers values" });
                    return;
                }
            }

            await next(context);
        }
    }
}
