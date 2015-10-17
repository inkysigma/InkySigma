using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Infrastructure.ErrorHandler;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Framework.Logging;

namespace InkySigma.Infrastructure.Filter
{
    public class ExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IExceptionPage _page;

        public ExceptionFilter(ILogger logger, IExceptionPage page)
        {
            _logger = logger;
            _page = page;
        }

        public void OnException(ExceptionContext context)
        {
            var httpContext = context.HttpContext;
            if (httpContext.Response.HasStarted)
                return;
            if (context.Exception is ArgumentNullException || context.Exception is FormatException)
            {
                var page = SetupPage(httpContext, _page, context.Exception, 400);
                WritePage(httpContext, page);
            }
            else if (context.Exception is InvalidUserException)
            {
                var page = SetupPage(httpContext, _page, context.Exception, 401);
                WritePage(httpContext, page);
            }
            else if (context.Exception is SqlException)
            {
                var page = SetupPage(httpContext, _page, context.Exception, 503);
                WritePage(httpContext, page);
                _logger.LogError(context.Exception.HResult, context.Exception.Message, context.Exception);
            }
            else
            {
                httpContext.Response.StatusCode = 503;
                _logger.LogError(context.Exception.HResult, context.Exception.Message, context.Exception);
            }
        }

        private string SetupPage(HttpContext httpContext, IExceptionPage page, Exception exception, int statusCode)
        {
            foreach (var i in page.Headers)
            {
                if (httpContext.Response.Headers.ContainsKey(i.Key))
                    continue;
                httpContext.Response.Headers[i.Key] = i.Value;
            }
            httpContext.Response.StatusCode = statusCode;

            page.SetException(exception);

            return page.Render().Replace(@"\", string.Empty);
        }

        private void WritePage(HttpContext httpContext, string page)
        {
            var task = Task.Run(async () => { await WritePageAsync(httpContext, page, CancellationToken.None); });
            task.Wait();
        }

        private async Task WritePageAsync(HttpContext httpContext, string page, CancellationToken token)
        {
            await httpContext.Response.WriteAsync(page, Encoding.UTF8, token);
            await httpContext.Response.Body.FlushAsync(token);
            httpContext.Response.Body.Close();
        }
    }
}