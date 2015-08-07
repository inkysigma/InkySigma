using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor;
using RazorEngine;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public class RazorErrorPage : IErrorPage
    {
        private string _path { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public RazorErrorPage(string path, Dictionary<string, string> headers)
        {
            _path = path;
            Headers = headers;
        }

        public string Render()
        {
        }
    }
}
