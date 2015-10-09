using System.Collections.Generic;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public class PlainErrorPage : IErrorPage
    {
        public PlainErrorPage(string value, Dictionary<string, string> headers = null)
        {
            Value = value;
            Headers = headers ?? new Dictionary<string, string>();
            Headers["Content-Type"] = "text/plain";
        }

        private string Value { get; }
        public Dictionary<string, string> Headers { get; set; }

        public string Render()
        {
            return Value;
        }
    }
}