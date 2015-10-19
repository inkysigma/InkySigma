using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Messages;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using InkySigma.Infrastructure.Exceptions;
using InkySigma.Model;
using InkySigma.ViewModel;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Controllers
{
    public class AuthorizationController : Controller
    {
        public UserManager<User> UserManager { get; set; }
        public IEmailService EmailService { get; set; }

        public AuthorizationController(UserManager<User> userManager, IEmailService emailService)
        {
            UserManager = userManager;
            EmailService = emailService;
        }

        
            // GET: api/values
        [HttpPost]
        public IEnumerable<string> Login()
        {
            return new[] {"value1", "value2"};
        }
        
        [HttpPost]
        public async Task<StandardResponse> Register(RegisterViewModel user)
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
    }
}
