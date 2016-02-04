using System.Threading.Tasks;
using InkySigma.Authentication.Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Messages;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using InkySigma.Web.Infrastructure.Exceptions;
using InkySigma.Web.Model;
using InkySigma.Web.RequestModel;
using InkySigma.Web.ResponseModels;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        public UserManager<User> UserManager { get; set; }
        public LoginService<User> LoginService { get; set; } 
        public IEmailService EmailService { get; set; }

        public AuthenticationController(UserManager<User> userManager, LoginService<User> loginService, IEmailService emailService)
        {
            UserManager = userManager;
            LoginService = loginService;
            EmailService = emailService;
        }

        
            // GET: api/values
        [HttpPost]
        public async Task<LoginResponseModel> Login(LoginRequestModel login)
        {
            if (login == null)
                throw new ParameterNullException(nameof(login));
            var token =
                await
                    LoginService.CreateLogin(login.UserName, login.Password, HttpContext.Request.Host.ToString(),
                        login.IsPersistent);
            if (token == null)
                return new LoginResponseModel
                {
                    LoginSucceeded = false
                };
            return new LoginResponseModel
            {
                Token = token,
                Name = (await UserManager.FindUserByUsername(login.UserName)).UserName
            };
        }

        [HttpPost]
        public async Task<StandardResponse> Register(RegisterRequestModel user)
        {
            if (user == null)
                throw new ParameterNullException(nameof(user));
            var constructed = user.Generate();
            var model = await UserManager.AddUser(constructed);
            await
                EmailService.SendEmail(new ActivationEmail(user.Email, model.Token, "api.inkysigma.com/activate",
                    "inkysigma.com/activate"));
            var response = new StandardResponse
            {
                Succeeded = true,
                Code = 200,
                Message = "Please check your email for a confirmation email",
                Payload = null
            };
            return response;
        }

        [HttpGet]
        public string Test()
        {
            return "Hello";
        }
    }
}