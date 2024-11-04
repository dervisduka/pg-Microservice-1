using Serilog.Context;

namespace WebApi.Middlewares
{
    public class LogUserNameMiddleware
    {
        private readonly RequestDelegate next;

        public LogUserNameMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("perdoruesi", context.User.Identity.Name ?? "anonymous"))
            {
                await next(context);
            }
        }
    }
}
