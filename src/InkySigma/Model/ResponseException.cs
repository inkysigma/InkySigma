using System;

namespace InkySigma.Model
{
    public class ResponseException
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ResponseException InnerException { get; set; }

        public ResponseException(Exception exception)
        {
            Code = exception.HResult;
            Message = exception.Message;
            if(exception.InnerException != null)
                InnerException = new ResponseException(exception.InnerException);
        }
    }
}
