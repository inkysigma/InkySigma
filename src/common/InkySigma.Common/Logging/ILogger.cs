namespace InkySigma.Common.Logging
{
    public interface ILogger
    {
        void LogInformation(LogMessage message);
        void LogError(LogMessage message, CommonException exception);
        void LogWarning(LogMessage message);
        void LogCritical(LogMessage message);
        void Log(LogMessage message);
    }
}
