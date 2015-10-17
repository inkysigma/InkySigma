using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Formatters;

namespace InkySigma.Infrastructure.Formatters
{
    public class JsonStandardMediaTypeFormatter : JsonOutputFormatter
    {
        public override async Task WriteResponseBodyAsync(OutputFormatterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var type = context.Object.GetType();
        }
    }
}
