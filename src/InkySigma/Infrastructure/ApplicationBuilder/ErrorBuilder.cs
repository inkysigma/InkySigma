using InkySigma.Infrastructure.ErrorHandler;
using Microsoft.AspNet.Builder;

namespace InkySigma.Infrastructure.ApplicationBuilder
{
    public static class ErrorBuilder
    {
        public static void UseCustomErrors(this IApplicationBuilder builder)
        {
            builder.UseErrorHandler(404, new PlainErrorPage("404"), WebService.Api);
            builder.UseErrorHandler(503, new PlainErrorPage("503"), WebService.Api);
            builder.UseErrorHandler(510, new PlainErrorPage("510"), WebService.Api);
            builder.UseErrorHandler(400, new PlainErrorPage("400"), WebService.Api);
            builder.UseErrorHandler(401, new PlainErrorPage("401"), WebService.Api);
        }
    }
}
