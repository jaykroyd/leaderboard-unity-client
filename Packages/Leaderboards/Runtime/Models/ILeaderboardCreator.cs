using UnityEngine.Events;

namespace Leaderboards
{
    public interface ILeaderboardCreator
    {
        void CreateHighscoreLeaderboard(string name, long capacity, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed);
        void CreateIncrementalLeaderboard(string name, long capacity, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed);
    }
}
