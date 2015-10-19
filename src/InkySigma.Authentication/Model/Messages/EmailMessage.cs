namespace InkySigma.Authentication.Model.Messages
{
    public class EmailMessage
    {
        public virtual string Recipient { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual string Alternate { get; set; }
        public virtual string ContentType { get; set; } = "html";
    }
}