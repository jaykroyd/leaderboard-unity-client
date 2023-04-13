namespace Leaderboards
{
    public struct GetLeaderboardResponse : IResponse
    {
        public LeaderboardModel leaderboard;
        public string error;

        public string Error => error;
        bool IResponse.IsError { get; set; }
    }
}
