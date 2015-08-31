using System;
using System.Text;
using System.Threading.Tasks;
using InkySigma.Infrastructure.ErrorHandler;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
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
            if (context.Exception is ArgumentNullException || context.Exception is FormatException)
            {
                if (httpContext.Response.HasStarted)
                    return;
                _page.SetException(context.Exception);
                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "application/json";
                var page = _page.Render().Replace(@"\", string.Empty);
                var task = Task.Run(async () =>
                {
                    await httpContext.Response.WriteAsync(page, Encoding.UTF8);
                    await httpContext.Response.Body.FlushAsync();
                    httpContext.Response.Body.Close();
                });
                task.Wait();
            }
            else
            {
                httpContext.Response.StatusCode = 503;
                _logger.LogError(context.Exception.HResult, context.Exception.Message, context.Exception);
            }
        }
    }
}
