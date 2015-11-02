using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InkySigma.Web.Infrastructure.ErrorHandler
{
    public class JsonErrorPage : IErrorPage
    {
        public JsonErrorPage(dynamic model)
        {
            Model = model;
        }

        public dynamic Model { get; set; }
        public Exception Exception { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public string Render()
        {
            return JsonConvert.SerializeObject(Model);
        }
    }
}