using System.Collections.Generic;
using InkySigma.Infrastructure.ErrorHandler;
using Microsoft.AspNet.Builder;

namespace InkySigma.ApplicationBuilder
{
    public static class ErrorBuilder
    {
        public static void UseCustomErrors(this IApplicationBuilder builder, string domain)
        {
            builder.UseErrorHandler(404, new PlainErrorPage("404"), WebService.Api);
            builder.UseErrorHandler(503, new PlainErrorPage("503"), WebService.Api);
            builder.UseErrorHandler(510, new PlainErrorPage("510"), WebService.Api);
            builder.UseErrorHandler(400, new PlainErrorPage("400"), WebService.Api);

            var errorPage = new PlainErrorPage("401", new Dictionary<string, string>
            {
                {"WWW-Authenticate", $"BASIC realm=\"{domain}\""}
            });
            builder.UseErrorHandler(401, errorPage, WebService.Api);
        }
    }
}
