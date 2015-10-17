using System;
using System.Collections.Generic;
using InkySigma.Common;

namespace InkySigma.Infrastructure.ExceptionPage
{
    public interface IExceptionPage
    {
        Dictionary<string, string> Headers { get; set; }
        string Render();
        void SetException(Exception exception);
    }
}