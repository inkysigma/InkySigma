using System.Data.Common;

namespace InkySigma.Authentication.AspNet
{
    public interface IAuthenticationBuilder<TUser>
    {
        IServiceCollection ServiceCollection { get; set; }
        
        DbConnection Connection { get; set; }
        
        RepositoryOptions RepositoryOptions { get; set; }
        
        LoginManagerOptions LoginOptions { get; set; }
        
        IEmailProvider EmailProvider { get; set; }
        
        TimeSpan ExpirationTime { get; set; } = TimeSpan.FromDays(1)
        
        Logger<UserManager<TUser>> UserLogger { get; set; }
        
        Logger<LoginManager<TUser>> LoginLogger { get; set; }
    }
}
