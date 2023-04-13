using UnityEngine;

namespace Leaderboards
{
    public struct CreateLeaderboardResponse : IResponse
    {
        public LeaderboardModel leaderboard;
        public string error;

        public string Error => error;
        bool IResponse.IsError { get; set; }
    }
}
