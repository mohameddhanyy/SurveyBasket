namespace SurveyBasket.Api.Middlewares
{
    public class CustomMiddlewares
    {
        private readonly ILogger<CustomMiddlewares> _logger;
        private readonly RequestDelegate _next;

        public CustomMiddlewares(ILogger<CustomMiddlewares> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Process Request");

            await _next(context);

            _logger.LogInformation("Process Response");
        }
    }
}
