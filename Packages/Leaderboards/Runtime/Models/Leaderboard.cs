using System;

namespace Leaderboards
{
    public class Leaderboard
    {
        public Guid ID { get; internal set; }
        public string Name { get; internal set; }
        public int Capacity { get; internal set; }
        public int Mode { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
        public DateTime CreatedAt { get; internal set; }

        public string ModeString => Mode == 0
            ? "Highscore"
            : "Incremental";

        public override string ToString()
        {
            return $"ID: {ID.ToString()}\nName: {Name}\nCapacity: {Capacity}\nMode: {ModeString}\nUpdatedAt: {UpdatedAt.ToString()}\nCreatedAt: {CreatedAt.ToString()}";
        }
    }
}
