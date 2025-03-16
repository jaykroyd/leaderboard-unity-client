namespace Leaderboards
{
    public struct UpdateParticipantScoreResponse : IResponse
    {
        public LeaderboardParticipantModel participant;
        public string error;

        public string Error => error;
        bool IResponse.IsError { get; set; }
    }
}
