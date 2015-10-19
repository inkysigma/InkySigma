using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Authentication.Model.Messages
{
    public sealed class ActivationEmail : EmailMessage
    {
        public ActivationEmail(string recipient, string code, string activationPoint, string activationPointClient)
        {
            Recipient = recipient;
            Subject = "Activation Email";
            var text = File.ReadAllText("./Templates/Activate.html");
            text = text.Replace(@"{{activationPoint}}", activationPoint);
            text = text.Replace("@{{code}}", code);
            Body = text.Replace(@"{{activationPointClient}}", activationPointClient);
            Alternate = $"Please enter {code} at {activationPointClient} to activate your account.";
            ContentType = "text/html";
        }
    }
}
