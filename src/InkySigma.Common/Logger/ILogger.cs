using System;

namespace InkySigma.Common.Logger
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public interface ILogger
    {
        void LogError(int code, string message, Exception error);
        void LogCritical(int code, string message);
    }
}