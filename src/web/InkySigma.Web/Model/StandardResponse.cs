namespace InkySigma.Web.Model
{
    /// <summary>
    /// This is a standard response for all of the controllers.
    /// First note that the code represents an HTTP code representation
    /// The message is a user friendly message
    /// Information is any further information
    /// </summary>
    public class StandardResponse
    {
        /// <summary>
        /// This boolean specifically refers to whether the operation was completed. Not whether the operation was valid.
        /// For example, a user being forbidden from an area would generate a Succeeded = false message, but a request for an incorrect 
        /// username and password combination will complete.
        /// In general, if the behavior is expected by user input, then this should return true and the Payload should
        /// contain further information. However, if the fault lies with the application, then this should return false
        /// </summary>
        public bool Succeeded { get; set; }

        public bool MessageAvailable => !string.IsNullOrEmpty(Message);

        /// <summary>
        /// This code should return 200 if the operation was completed successfully. However, check this code for other options as well.
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// User friendly messages from the server.
        /// </summary>
        public string Message { get; set; }
        public string Information { get; set; }

        /// <summary>
        /// Developer information for debugging purposes.
        /// </summary>
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
