using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace InkySigma.Identity.Validator
{
    public class EmailValidator : IValidator
    {
        public IEnumerable<string> Validate(string input)
        {
            var problems = new List<string>();
            if (string.IsNullOrEmpty(input))
            {
                problems.Add("Email cannot be empty");
                return problems;
            }
            var regex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
            var matches = regex.Matches(input);
            if(matches.Count!=1)
                problems.Add("Email is invalid");
            if (problems.Count == 0)
                return null;
            return problems;
        }
    }
}
