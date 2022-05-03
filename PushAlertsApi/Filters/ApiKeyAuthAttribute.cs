using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PushAlertsApi.Filters
{
    /// <summary>
    /// This class provides a custom filter attribute to enable ApiKey authentication for an API Endpoint.
    /// It checks whether the given ApiKey is equal to the one in the configuration file aka appsettings.json.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";

        /// <summary>
        /// This method works as a kind of middleware for API requests. It is executed before the actual API Controller method.
        /// </summary>
        /// <param name="context">The application context as HTTP request</param>
        /// <param name="next">Delegate which continues with the API Controller method when executed</param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>(ApiKeyHeaderName);

            if (!apiKey.Equals(potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next(); // Api Controller
        }
    }
}
