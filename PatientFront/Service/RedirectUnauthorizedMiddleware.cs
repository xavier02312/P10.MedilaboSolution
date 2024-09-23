namespace PatientFront.Service
{
    public class RedirectUnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectUnauthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden || context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.Redirect("/Home/Error/404");
            }
        }
    }
}
