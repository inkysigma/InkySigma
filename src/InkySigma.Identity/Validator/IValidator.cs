using System.Collections.Generic;

namespace InkySigma.Identity.Validator
{
    public interface IValidator
    {
        IEnumerable<string> Validate(string input);
    }
}
