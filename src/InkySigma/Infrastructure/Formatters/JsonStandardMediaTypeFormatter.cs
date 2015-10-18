using System;
using System.Threading.Tasks;
using InkySigma.Model;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;

namespace InkySigma.Infrastructure.Formatters
{
    public class JsonStandardMediaTypeFormatter : JsonOutputFormatter
    {
        public override Task WriteResponseBodyAsync(OutputFormatterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;
            var encoding = context.SelectedEncoding;

            StandardResponse standard;

            var o = context.Object as StandardResponse;
            if (o != null)
                standard = o;
            else
                standard = new StandardResponse
                {
                    Code = 200
                };
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
