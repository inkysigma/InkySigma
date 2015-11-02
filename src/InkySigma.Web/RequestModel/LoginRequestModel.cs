namespace InkySigma.Web.RequestModel
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
