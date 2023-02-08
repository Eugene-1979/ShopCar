using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace _07_CustomMiddlewareComponent
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next; // представляет следующие middleware в конвейере

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("CUSTOM-HEADER", "CUSTOM VALUE");
                return Task.CompletedTask;
            });

            await _next(context); // вызов оставшихся middleware в pipeline
        }
    }
}
