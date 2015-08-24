namespace InkySigma.Identity.Dapper.Models
{
    public class UserClaimsViewModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public dynamic Payload { get; set; }
    }
}
