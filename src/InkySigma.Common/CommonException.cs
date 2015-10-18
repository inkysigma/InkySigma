using System;

namespace InkySigma.Common
{
    public class CommonException : Exception
    {
        public int Code { get; set; }
        public string Information { get; set; }
        public string Developer { get; set; }

        public CommonException(int code, string message, string information, string developer) : base(message)
        {
            Code = code;
            Information = information;
            Developer = developer;
        }
    }
}
