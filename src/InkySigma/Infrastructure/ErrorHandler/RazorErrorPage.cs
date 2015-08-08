using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.Runtime;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public class RazorErrorPage : IErrorPage
    {
        private string _path { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public IApplicationEnvironment Environment { get; set; }

        public RazorErrorPage(string path, IApplicationEnvironment environment, Dictionary<string, string> headers = null)
        {
            _path = path;
            Headers = headers ?? new Dictionary<string, string>();
            Environment = environment;
        }

        public string Render()
        {
            if(!File.Exists(Environment))
        }
    }
}
