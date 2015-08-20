using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Validator
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
