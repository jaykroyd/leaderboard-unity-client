using UnityEngine.Events;

namespace Leaderboards
{
    public interface ILeaderboardCreator
    {
        void CreateHighscoreLeaderboard(string name, int capacity, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed);
        void CreateIncrementalLeaderboard(string name, int capacity, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed);
    }
}
