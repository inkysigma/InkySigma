using System;
using System.Collections.Generic;

namespace InkySigma.Web.Infrastructure.ExceptionPage
{
    public interface IExceptionPage
    {
        Dictionary<string, string> Headers { get; set; }
        string Render();
        void SetException(Exception exception);
    }
}