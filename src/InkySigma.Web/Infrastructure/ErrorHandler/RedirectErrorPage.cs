using System.Collections.Generic;

namespace InkySigma.Web.Infrastructure.ErrorHandler
{
    public class RedirectErrorPage : IErrorPage
    {
        public RedirectErrorPage(string path, string _base, Dictionary<string, string> headers = null)
        {
            Headers = headers ?? new Dictionary<string, string>();
            Headers["Location"] = _base + path;
        }

        public Dictionary<string, string> Headers { get; set; }

        public string Render()
        {
            return "";
        }
    }
}