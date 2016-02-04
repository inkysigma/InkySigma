namespace InkySigma.Web.Core
{
    public class ContactRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }

        public SigmaUser Target { get; set; }
    }
}
