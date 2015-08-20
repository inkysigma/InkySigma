using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Validator
{
    public class PasswordValidator : IValidator
    {
        public IEnumerable<string> Validate(string input)
        {
            var problems = new List<string>();

            if (string.IsNullOrEmpty(input))
            {
                problems.Add("Password cannot be empty");
                return problems;
            }

            if (input.Any(c => c >= 'a' && c <= 'z') && input.Any(c => c >= '0' && c <= '9'))
            {
                problems.Add("Password requires a number and lowercase letter.");
            }

            if (problems.Count == 0)
                return null;
            return problems;
        }
    }
}
