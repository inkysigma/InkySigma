namespace InkySigma.Web.Business.Options
{
    public class TableOptions
    {
        public string ContactTable { get; set; } = "user.contacts";
        public string ContactRequestTable { get; set; } = "user.requests";
        public string DefaultTable { get; set; } = "user.default";
    }
}
