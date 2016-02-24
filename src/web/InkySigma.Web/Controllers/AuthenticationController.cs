using System.Threading.Tasks;
using InkySigma.Authentication.Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Messages;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using InkySigma.Web.Core;
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
        public UserService<SigmaUser> UserService { get; set; }
        public LoginService<SigmaUser> LoginService { get; set; } 
        public IEmailService EmailService { get; set; }

        public AuthenticationController(UserService<SigmaUser> userService, LoginService<SigmaUser> loginService, IEmailService emailService)
        {
            UserService = userService;
            LoginService = loginService;
            EmailService = emailService;
        }

        
        /// <summary>
        /// Logs a user in given a LoginRequestModel.
        /// </summary>
        /// <param name="login">The login information</param>
        /// <returns>A response model containing the </returns>
        [HttpPost]
        public async Task<LoginResponseModel> Login([FromBody] LoginRequestModel login)
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
                Name = (await UserService.FindUserByUsername(login.UserName)).UserName
            };
        }

        [HttpPost]
        public async Task<StandardResponse> Register([FromBody] RegisterRequestModel user)
        {
            if (user == null)
                throw new ParameterNullException(nameof(user));
            var constructed = user.Generate();
            var model = await UserService.AddUser(constructed);
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
    }
}