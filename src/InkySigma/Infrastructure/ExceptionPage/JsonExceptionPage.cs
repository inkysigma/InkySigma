using System;
using System.Collections.Generic;
using InkySigma.Common;
using InkySigma.Model;
using Newtonsoft.Json;

namespace InkySigma.Infrastructure.ExceptionPage
{
    public class JsonExceptionPage : IExceptionPage
    {
        protected CommonException Exception { get; set; }

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>
        {
            {"Content-Type", "application/json"}
        };

        public string Render()
        {
            var page = new StandardResponse()
            {
                Succeeded = false,
                Code = Exception.Code,
                Information = Exception.Information,
                Message = Exception.Message,
                Payload = null
            };
            return JsonConvert.SerializeObject(page);
        }

        public void SetException(Exception exception)
        {
            var commonException = exception as CommonException;
            if (commonException != null)
            {
                Exception = commonException;
                return;
            }
            Exception = new CommonException(503, exception.Message, null);
        }
    }
}