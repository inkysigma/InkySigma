using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Common;

namespace InkySigma.Authentication.AspNet
{
    public class HeaderFormatException : CommonException
    {
        public HeaderFormatException(int code, string information,
            string developer = "Incorrect headers were sent")
            : base(code, "The developer has made an error", information, developer)
        {
        }
    }
}
