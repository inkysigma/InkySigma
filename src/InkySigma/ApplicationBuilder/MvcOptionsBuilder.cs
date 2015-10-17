﻿using InkySigma.Infrastructure.ErrorHandler;
using InkySigma.Infrastructure.ExceptionHandler;
using InkySigma.Infrastructure.Filter;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace InkySigma.ApplicationBuilder
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