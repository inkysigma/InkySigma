using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public delegate bool WebServiceType(HttpContext context);

    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class ErrorHandler
    {
        private readonly WebServiceType _check;
        private readonly RequestDelegate _next;
        private readonly IErrorPage _page;
        private readonly int _status;

        public ErrorHandler(RequestDelegate next, int status, IErrorPage page, WebServiceType check)
        {
            _next = next;
            _page = page;
            _status = status;
            _check = check;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);

            var response = httpContext.Response;
            if (response.HasStarted
                || response.StatusCode < 400
                || response.StatusCode >= 600
                || response.StatusCode != _status
                || response.ContentLength.HasValue
                || !string.IsNullOrEmpty(response.ContentType)
                || !_check(httpContext))
                return;

            _page.Headers.ToList().ForEach(c => response.Headers[c.Key] = c.Value);
            await response.WriteAsync(_page.Render());
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder, int statusCode,
            IErrorPage page, WebServiceType type)
        {
            return builder.UseMiddleware<ErrorHandler>(statusCode, page, type);
        }
    }

    public class WebService
    {
        public static bool Api(HttpContext context)
        {
            var request = context.Request;
            var requestUrl = UriHelper.Encode(request.Scheme, request.Host, request.PathBase, request.Path,
                request.QueryString);
            var match = Regex.IsMatch(requestUrl, @"^http(s)?:\/\/\S+(\.\S+)?(\\|\/)api(\\|\/)?(\S+)?$");
            return match;
        }

        public static bool Mvc(HttpContext context)
        {
            var request = context.Request;
            var requestUrl = UriHelper.Encode(request.Scheme, request.Host, request.PathBase, request.Path,
                request.QueryString);
            var requestHost = UriHelper.Encode(request.Scheme, request.Host);
            Console.WriteLine(requestHost);
            return !requestUrl.StartsWith(requestHost + "/api");
        }
    }
}