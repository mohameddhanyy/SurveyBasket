namespace SurveyBasket.Api.Middlewares
{
    public static class ExtensionCustomMiddleware
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomMiddlewares>();
        }
    }
}
