using InkySigma.Authentication.Dapper.Models;
using InkySigma.Web.Core;

namespace InkySigma.Web.RequestModel
{
    public class RegisterRequestModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public SigmaUser Generate()
        {
            var user = new SigmaUser
            {
                Email = Email,
                Name = Name,
                Password = Password,
                UserName = UserName
            };
            return user;
        }
    }
}
