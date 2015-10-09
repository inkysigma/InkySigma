using System.Collections.Generic;
using InkySigma.Infrastructure.Filter;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Controllers
{
    public class AuthorizationController : Controller
    {
        // GET: api/values
        [HttpPost]
        [IdentityAuthorize("Temp")]
        public IEnumerable<string> Login()
        {
            return new[] {"value1", "value2"};
        }

        [HttpGet]
        public IEnumerable<string> Test()
        {
            return new[] {"hello", "ji"};
        }
    }
}