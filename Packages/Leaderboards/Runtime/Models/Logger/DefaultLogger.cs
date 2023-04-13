namespace Leaderboards
{
    public class DefaultLogger : ILogger
    {
        public void LogErrorToUnity(string message)
        {
            UnityEngine.Debug.LogError($"[Leaderboards] {message}");
        }

        public void LogToUnity(string message)
        {
            UnityEngine.Debug.Log($"[Leaderboards] {message}");
        }
    }
}
