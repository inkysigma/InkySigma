using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Controllers
{
    public class AuthorizationController : Controller
    {
        // GET: api/values
        [HttpPost]
        public IEnumerable<string> Login()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
