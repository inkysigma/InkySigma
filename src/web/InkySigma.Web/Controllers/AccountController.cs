using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Messages;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using InkySigma.Web.Core;
using InkySigma.Web.Infrastructure.Exceptions;
using InkySigma.Web.RequestModel;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Web.Controllers
{
    public class AccountController : Controller
    {
        public UserService<SigmaUser> UserService { get; }
        public LoginService<SigmaUser> LoginService { get; }
        public IEmailService EmailService { get; }

        public AccountController(UserService<SigmaUser> userService, LoginService<SigmaUser> loginService, IEmailService emailService)
        {
            UserService = userService;
            LoginService = loginService;
            EmailService = emailService;
        }

        [HttpPost]
        public async Task ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            if (model == null)
                throw new ParameterNullException(nameof(ResetPasswordRequestModel));
            if (model.UserName == null)
                throw new InvalidParameterException(nameof(model.UserName));
            var user = await UserService.FindUserByUsername(model.UserName);
            var token = await UserService.RequestPasswordResetAsync(user);
            var reciepient = await UserService.GetUserEmailAsync(user);
            await EmailService.SendEmail(new EmailMessage
            {
                Subject = "Reset Password",
                Alternate = $"Please use the token {token} at www.inkysigma.com/reset to reset your password",
                Body = $@"<html><body>Someone has recently tried to reset your password. If this was you, click <a href='www.inkysigma.com/reset/{token}'>here</a>",
                ContentType = "text/html",
                Recipient = reciepient
            });
        }
    }
}
