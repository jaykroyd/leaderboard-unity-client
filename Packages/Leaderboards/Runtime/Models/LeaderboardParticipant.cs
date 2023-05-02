using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace Leaderboards
{
    public struct LeaderboardParticipant
    {
        public Guid LeaderboardID { get; internal set; }
        public string ExternalID { get; internal set; }
        public string Name { get; internal set; }
        public int Rank { get; internal set; }
        public long Score { get; internal set; }
        public IReadOnlyDictionary<string, string> Metadata { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
        public DateTime CreatedAt { get; internal set; }

        public override string ToString()
        {
            return $"Leaderboard:{LeaderboardID.ToString()}\nID: {ExternalID}\nName: {Name}\nRank: {Rank}\nScore: {Score}\nMetadata: {JsonConvert.SerializeObject(Metadata)}\nUpdatedAt: {UpdatedAt.ToString()}\nCreatedAt: {CreatedAt.ToString()}";
        }
    }
}
