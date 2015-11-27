using System.Data.Common;

namespace InkySigma.Authentication.AspNet
{
    public interface IAuthenticationBuilder
    {
        DbConnection Connection { get; set; }
    }
}
