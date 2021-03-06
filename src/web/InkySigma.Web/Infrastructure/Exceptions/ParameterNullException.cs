﻿using InkySigma.Common;
using InkySigma.Common.Exceptions;

namespace InkySigma.Web.Infrastructure.Exceptions
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
