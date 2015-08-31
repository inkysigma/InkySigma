using System;
using InkySigma.Model;
using Newtonsoft.Json;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public class JsonExceptionPage : IExceptionPage
    {
        protected Exception Exception { get; set; }

        public string Render()
        {
            var page = new ResponseFrameModel()
            {
                Succeeded = false,
                Exception = JsonConvert.SerializeObject(new ResponseException(Exception)).Replace("\r\n", " "),
                Payload = null
            };
            return JsonConvert.SerializeObject(page);
        }

        public void SetException(Exception exception)
        {
            Exception = exception;
        }
    }
}
