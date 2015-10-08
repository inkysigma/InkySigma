using System.Collections.Generic;

namespace InkySigma.Authentication.Validator
{
    public class UsernameValidator : IValidator
    {
        public IEnumerable<string> Validate(string input)
        {
            var problems = new List<string>();

            if (string.IsNullOrEmpty(input))
            {
                problems.Add("Username cannot be empty");
                return problems;
            }

            if (input.Length < 8)
            {
                problems.Add("Username cannot be less than 8 characters");
            }
            
            if(problems.Count == 0)
                return null;

            return problems;
        }
    }
}
