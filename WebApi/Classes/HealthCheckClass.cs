using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace WebApi.Classes
{
    public class HealthCheckClass
    {
        public static Task WriteHealthResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new
            {
                status = result.Status.ToString(),
                results = result.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    exception = e.Value.Exception?.Message
                })
            });
            return context.Response.WriteAsync(json);
        }
    }
}
