using InkySigma.Infrastructure.ExceptionPage;
using InkySigma.Infrastructure.Filters;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace InkySigma.ApplicationBuilders
{
    public static class MvcOptionsBuilder
    {
        public static IServiceCollection ConfigureMvcOptions(this IServiceCollection collection)
        {
            var logger = new Logger<ExceptionContext>(new LoggerFactory());
            collection.Configure<MvcOptions>(
                options => { options.Filters.Add(new ExceptionFilter(logger, new JsonExceptionPage())); });
            return collection;
        }
    }
}