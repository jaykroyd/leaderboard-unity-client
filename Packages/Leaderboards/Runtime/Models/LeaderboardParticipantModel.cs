using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Leaderboards
{
    public struct LeaderboardParticipantModel
    {
        public int rank;
        public string leaderboard_id;
        public string external_id;
        public string name;        
        public long score;
        public Dictionary<string, string> metadata;
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
                LeaderboardID = Guid.Parse(leaderboard_id),
                ExternalID = external_id,
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
