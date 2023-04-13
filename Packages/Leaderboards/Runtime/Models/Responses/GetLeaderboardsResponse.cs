namespace Leaderboards
{
    public struct GetLeaderboardsResponse : IResponse
    {
        public LeaderboardModel[] leaderboards;
        public string error;

        public string Error => error;
        bool IResponse.IsError { get; set; }
    }
}
