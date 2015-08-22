using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Identity.RandomProvider;

namespace InkySigma.Identity.Models
{
    public class RandomOptions
    {
        public IUserIdProvider UserIdProvider { get; set; } = new UserIdProvider();
        public ITokenProvider TokenProvider { get; set; } = new TokenProvider();
    }
}
