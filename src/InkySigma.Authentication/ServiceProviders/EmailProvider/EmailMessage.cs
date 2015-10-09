namespace InkySigma.Authentication.ServiceProviders.EmailProvider
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Alternate { get; set; }
        public string ContentType { get; set; } = "html";
    }
}