using System;
using System.Collections.Generic;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public interface IExceptionPage
    {
        Dictionary<string, string> Headers { get; set; }

        string Render();

        void SetException(Exception exception);
    }
}
