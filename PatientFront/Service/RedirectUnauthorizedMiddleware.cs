using Serilog;

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
            try
            {
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Response.Redirect("/Home/Error/401");
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    context.Response.Redirect("/Home/Error/403");
                }
                else if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Response.Redirect("/Home/Error/404");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Log.Error(ex, "An error occurred while processing the request.");

                // Redirect to a generic error page
                context.Response.Redirect("/Home/Error");
            }
        }
    }
}
