using System;
using System.Text;
using System.Threading.Tasks;
using InkySigma.Web.Model;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;

namespace InkySigma.Web.Infrastructure.Formatters
{
    public class JsonStandardMediaTypeFormatter : JsonOutputFormatter
    {
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;
            var encoding = context.ContentType?.Encoding ?? Encoding.UTF8;

            var o = context.Object as StandardResponse;
            var standard = o ?? StandardResponse.Create(context.Object);

            return Task.Run(() =>
            {
                using (var stream = new HttpResponseStreamWriter(response.Body, encoding))
                {
                    WriteObject(stream, standard);
                }
            });
        }
    }
}
