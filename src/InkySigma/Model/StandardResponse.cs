namespace InkySigma.Model
{
    /// <summary>
    /// This is a standard response for all of the controllers.
    /// First note that the code represents an HTTP code representation
    /// The message is a user friendly message
    /// Information is any further information
    /// </summary>
    public class StandardResponse
    {
        public bool Succeeded { get; set; }

        public bool MessageAvailable => !string.IsNullOrEmpty(Message);

        public int Code { get; set; } = 200;
        public string Message { get; set; }
        public string Information { get; set; }
        public string Developer { get; set; }
        public dynamic Payload { get; set; }

        public static StandardResponse Create(dynamic payload)
        {
            return new StandardResponse
            {
                Succeeded = true,
                Payload = payload
            };
        } 
    }
}
