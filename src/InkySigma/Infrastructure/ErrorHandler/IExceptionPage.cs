using System;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public interface IExceptionPage
    {
        string Render();

        void SetException(Exception exception);
    }
}
