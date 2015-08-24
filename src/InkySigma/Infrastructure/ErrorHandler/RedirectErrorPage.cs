using System;
using System.Collections.Generic;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public class RedirectErrorPage : IErrorPage
    {
        public Dictionary<string, string> Headers { get; set; }

        public RedirectErrorPage(string path, string _base, Dictionary<string, string> headers = null)
        {
            Headers = headers ?? new Dictionary<string, string>();
            Headers["Location"] = _base + path;
        }

        public string Render()
        {
            return "";
        }
    }
}
