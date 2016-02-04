using System.Collections.Generic;
using Newtonsoft.Json;

namespace InkySigma.Authentication.Dapper.Models
{
    public class User
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsActive { get; set; }
        public bool LockoutEnabled { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Logins { get; set; }

        public static User Create(string id)
        {
            return new User
            {
                Id = id
            };
        }
    }
}