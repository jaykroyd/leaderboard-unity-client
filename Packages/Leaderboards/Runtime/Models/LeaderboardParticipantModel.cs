using System;
using System.Collections.Generic;

namespace Leaderboards
{
    public struct LeaderboardParticipantModel
    {
        public string id;
        public string name;
        public int rank;
        public long score;
        public Dictionary<string, object> metadata;
        public string updated_at;
        public string created_at;

        public LeaderboardParticipant ToLeaderboardParticipant()
        {
            var updated_at = DateTime.TryParse(this.updated_at, out DateTime updatedAt)
                ? updatedAt
                : new DateTime();

            var created_at = DateTime.TryParse(this.created_at, out DateTime createdAt)
                ? createdAt
                : new DateTime();

            return new LeaderboardParticipant
            {
                ID = id,
                Name = name,
                Rank = rank,
                Score = score,
                Metadata = metadata,
                UpdatedAt = updated_at,
                CreatedAt = created_at,
            };
        }
    }
}
