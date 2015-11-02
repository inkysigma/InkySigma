using System.Collections.Generic;

namespace InkySigma.Web.Infrastructure.ErrorHandler
{
    public interface IErrorPage
    {
        Dictionary<string, string> Headers { get; set; }
        string Render();
    }
}