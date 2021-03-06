﻿using System;
using System.Collections.Generic;
using InkySigma.Common;
using InkySigma.Common.Exceptions;
using InkySigma.Web.Model;
using Newtonsoft.Json;

namespace InkySigma.Web.Infrastructure.ExceptionPage
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
            var page = new StandardResponse
            {
                Succeeded = false,
                Code = Exception.Code,
                Information = Exception.Information,
                Message = Exception.Message,
                Developer = Exception.Developer,
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
            Exception = new CommonException(503, exception.Message, null, null);
        }
    }
}