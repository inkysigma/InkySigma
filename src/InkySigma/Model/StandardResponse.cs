namespace InkySigma.Model
{
    /// <summary>
    /// This is a standard response for all of the controllers.
    /// First note that the code represents an HTTP code representation
    /// The message is a user friendly message
    /// Information is any further information
    /// </summary>
    public class StandardResponse<T>
    {
        public bool Succeeded { get; set; }
        public int Code { get; set; } = 200;
        public string Message { get; set; }
        public string Information { get; set; }
        public T Payload { get; set; }

        public static StandardResponse<T> Create(T payload)
        {
            return new StandardResponse<T>
            {
                Succeeded = true,
                Payload = payload
            };
        } 
    }
}
