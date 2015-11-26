namespace InkySigma.Web.Core
{
    public class Contact
    {
        public string Id { get; set; }
        public SigmaUser Target { get; set; }
        public string Relation { get; set; }
    }
}
