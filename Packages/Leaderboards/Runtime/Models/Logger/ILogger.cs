namespace Leaderboards
{
    public interface ILogger
    {
        void LogToUnity(string message);
        void LogErrorToUnity(string message);
    }

}
