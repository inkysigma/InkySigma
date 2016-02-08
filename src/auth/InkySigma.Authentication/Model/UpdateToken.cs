using System;

namespace InkySigma.Authentication.Model
{
    public class UpdateToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UpdateProperty Property { get; set; } = UpdateProperty.Password;

        public bool Validate()
        {
            return string.IsNullOrEmpty(Token) && Expiration > DateTime.Now;
        }
    }

    public enum UpdateProperty
    {
        Password = 0,
        Activate = 1
    }
}