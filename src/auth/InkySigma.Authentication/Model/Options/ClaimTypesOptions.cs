using System.Security.Claims;

namespace InkySigma.Authentication.Model.Options
{
    public class ClaimTypesOptions
    {
        public string AuthClaimType { get; set; } = ClaimTypes.Role;
        public string UserIdType { get; set; } = ClaimTypes.NameIdentifier;
        public string UserNameClaimType { get; set; } = ClaimTypes.Name;
    }
}