namespace DAPM.ClientApi.Middleware
{
    public class GuestRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public GuestRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Post &&
                context.Request.Method != HttpMethods.Put &&    
                context.Request.Method != HttpMethods.Delete
                // Add more here in the future if needed
                ) {
                await _next(context);
                return;
            }

            if (!context.User.Identity.IsAuthenticated) {
                await _next(context);
                return;
            }

            if (context.User.IsInRole("Guest")) {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                // await context.Response.WriteAsync("Guests cannot perform this action.");
                return;
            }

            await _next(context);
        }
    }
}