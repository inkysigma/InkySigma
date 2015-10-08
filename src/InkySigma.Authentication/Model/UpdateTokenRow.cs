using System;

namespace InkySigma.Authentication.Model
{
    public class UpdateTokenRow
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UpdateProperty Property { get; set; } = UpdateProperty.Password;
    }

    public enum UpdateProperty
    {
        Password = 0,
        Activate = 1
    }
}
