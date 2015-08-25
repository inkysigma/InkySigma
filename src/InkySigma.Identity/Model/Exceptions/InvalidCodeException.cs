using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Model.Exceptions
{
    public class InvalidCodeException : Exception
    {
        public InvalidCodeException() : base("The given code was invalid.")
        {
            this.HResult = 5;
        }
    }
}
