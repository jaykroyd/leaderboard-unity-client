using System;
using System.Collections.Generic;

namespace Leaderboards
{
    public struct LeaderboardParticipant
    {
        public string ID { get; internal set; }
        public string Name { get; internal set; }
        public int Rank { get; internal set; }
        public long Score { get; internal set; }
        public IReadOnlyDictionary<string, object> Metadata { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
        public DateTime CreatedAt { get; internal set; }

        public override string ToString()
        {
            return $"ID: {ID.ToString()}\nName: {Name}\nRank: {Rank}\nScore: {Score}\nUpdatedAt: {UpdatedAt.ToString()}\nCreatedAt: {CreatedAt.ToString()}";
        }
    }
}
