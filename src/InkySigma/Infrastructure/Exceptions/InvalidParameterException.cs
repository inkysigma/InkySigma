using InkySigma.Common;

namespace InkySigma.Infrastructure.Exceptions
{
    public class InvalidParameterException : CommonException
    {
        public InvalidParameterException(string information)
            : base(
                400, "This app seems to be outdated. Try checking for updates.", information,
                "The parameter you gave for the action seems to be incomplete." + " Check information for more details")
        {
        }
    }
}
