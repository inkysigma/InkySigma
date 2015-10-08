using System.Collections.Generic;

namespace InkySigma.Authentication.Validator
{
    public interface IValidator
    {
        IEnumerable<string> Validate(string input);
    }
}
