using System;

namespace InkySigma.Common
{
    public class CommonException : Exception
    {
        public int Code { get; set; }
        public string Information { get; set; }

        public CommonException(int code, string message, string information) : base(message)
        {
            Code = code;
            Information = information;
        }
    }
}
