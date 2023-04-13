using System;
using UnityEngine.Events;

namespace Leaderboards
{
    public interface ILeaderboardProvider
    {
        void GetLeaderboard(Guid id, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed);
        void GetHighscoreLeaderboards(UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed);
        void GetIncrementalLeaderboards(UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed);
        void GetAllLeaderboards(UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed);
    }
}
