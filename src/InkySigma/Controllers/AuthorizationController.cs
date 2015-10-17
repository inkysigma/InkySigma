using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Infrastructure.Exceptions;
using InkySigma.ViewModel;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Controllers
{
    public class AuthorizationController : Controller
    {
        public UserManager<User> UserManager { get; set; }

        public AuthorizationController(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        
            // GET: api/values
        [HttpPost]
        public IEnumerable<string> Login()
        {
            return new[] {"value1", "value2"};
        }

        public async Task<string> Register(RegisterViewModel user)
        {
            if (user == null)
                throw new ParameterNullException(nameof(user));
            var constructed = user.Generate();
            return await UserManager.AddUser(constructed);
        }
    }
}