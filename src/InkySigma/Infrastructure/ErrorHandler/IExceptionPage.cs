using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public interface IExceptionPage
    {
        string Render();

        void SetException(Exception exception);
    }
}
