using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Common;

namespace InkySigma.Infrastructure.Exceptions
{
    public class ParameterNullException : CommonException
    {
        public ParameterNullException(string information)
            : base(
                400, "The application you are using has a problem. Please contact the developer", information,
                "The parameters provided do not match up with specifications.")
        {
        }
    }
}
