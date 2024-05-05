using Newtonsoft.Json;

namespace qrwaiter_backend.Middlewares
{
    public class ResponseErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
    
            if(context.Response.StatusCode == StatusCodes.Status401Unauthorized) {
            
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                //var errorMessage = JsonConvert.SerializeObject(new { error = ex.Message });
                //await context.Response.WriteAsync(errorMessage);
                await _next(context);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
