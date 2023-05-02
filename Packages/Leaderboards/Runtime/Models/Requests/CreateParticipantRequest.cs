using System.Collections.Generic;

namespace Leaderboards
{
    public struct CreateParticipantRequest
    {
        public string leaderboard_id;
        public string external_id;
        public string name;
        public Dictionary<string, string> metadata;
    }
}
