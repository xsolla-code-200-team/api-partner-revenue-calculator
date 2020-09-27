using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using xsolla_revenue_calculator.Exceptions;

namespace xsolla_revenue_calculator.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        public async Task Invoke(HttpContext httpContext)
        {
            var ex = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            int statusCode = 500;
            if (ex is ValidationException) statusCode = 400;
            else if (ex is ItemNotFoundException) statusCode = 404;
            var error = new ExceptionDetails(ex);
            error.StatusCode = statusCode;
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            await using var writer = new StreamWriter(httpContext.Response.Body);
            new JsonSerializer().Serialize(writer, error);
            await writer.FlushAsync().ConfigureAwait(false);
        }
    }
}