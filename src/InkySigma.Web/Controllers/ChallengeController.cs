using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Web.Controllers
{
    public class ChallengeController : Controller
    {
        [HttpPost]
        public async Task<string> RequestChallenge()
        {
            throw new NotImplementedException();
        }
    }
}
