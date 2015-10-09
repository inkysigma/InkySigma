using InkySigma.Authentication.Validator;

namespace InkySigma.Authentication.Model.Options
{
    public class FormValidatorOptions
    {
        public IValidator UsernameValidator { get; set; } = new UsernameValidator();
        public IValidator PasswordValidator { get; set; } = new PasswordValidator();
        public IValidator EmailValidator { get; set; } = new EmailValidator();
        public static FormValidatorOptions Default => new FormValidatorOptions();
    }
}