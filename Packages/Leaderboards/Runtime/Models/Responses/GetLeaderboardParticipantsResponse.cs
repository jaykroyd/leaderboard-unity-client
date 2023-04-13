namespace Leaderboards
{
    public struct GetLeaderboardParticipantsResponse : IResponse
    {
        public LeaderboardParticipantModel[] participants;
        public string error;

        public string Error => error;
        bool IResponse.IsError { get; set; }
    }
}
