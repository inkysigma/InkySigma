using InkySigma.Authentication.Dapper.Models;

namespace InkySigma.Web.RequestModel
{
    public class RegisterRequestModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public User Generate()
        {
            var user = new User
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
