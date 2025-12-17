namespace EmployeeManagementAPI
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class RequestGuidMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestGuidMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Generate a new GUID for each request
            var requestGuid = Guid.NewGuid().ToString();

            // Add the GUID to the request headers (can be used in controllers and logging)
            httpContext.Request.Headers["Request-Guid"] = requestGuid;

            // Proceed with the next middleware in the pipeline
            await _next(httpContext);
        }
    }

}
