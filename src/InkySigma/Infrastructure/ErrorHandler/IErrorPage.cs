using System.Collections.Generic;

namespace InkySigma.Infrastructure.ErrorHandler
{
    public interface IErrorPage
    {
        Dictionary<string, string> Headers { get; set; } 
        string Render();
    }
}
