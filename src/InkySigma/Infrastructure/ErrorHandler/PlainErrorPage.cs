using System.Collections.Generic;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public class PlainErrorPage : IErrorPage
    {
        private string Value { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public PlainErrorPage(string value, Dictionary<string, string> headers = null)
        {
            Value = value;
            Headers = headers ?? new Dictionary<string, string>();
            Headers["Content-Type"] = "text/plain";
        }

        public string Render()
        {
            return Value;
        }
    }
}
