using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public class JsonErrorPage : IErrorPage
    {
        public dynamic Model { get; set; }
        public Exception Exception { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public JsonErrorPage(dynamic model)
        {
            Model = model;
        }

        public string Render()
        {
            return JsonConvert.SerializeObject(Model);
        }
    }
}
