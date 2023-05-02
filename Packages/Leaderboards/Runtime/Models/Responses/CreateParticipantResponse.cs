namespace Leaderboards
{
    public struct CreateParticipantResponse : IResponse
    {
        public LeaderboardParticipantModel participant;
        public string error;

        public string Error => error;
        bool IResponse.IsError { get; set; }
    }
}
