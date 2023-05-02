using System;

namespace Leaderboards
{
    [System.Serializable]
    public struct LeaderboardModel
    {
        public string id;
        public string name;
        public int capacity;
        public int mode;
        public string updated_at;
        public string created_at;

        public Leaderboard ToLeaderboard()
        {
            var id = Guid.TryParse(this.id, out Guid guid)
                ? guid
                : Guid.Empty;

            var updated_at = DateTime.TryParse(this.updated_at, out DateTime updatedAt)
                ? updatedAt
                : new DateTime();

            var created_at = DateTime.TryParse(this.created_at, out DateTime createdAt)
                ? createdAt
                : new DateTime();

            return new Leaderboard
            {
                ID = id,
                Name = name,
                Capacity = capacity,
                Mode = mode,
                UpdatedAt = updated_at,
                CreatedAt = created_at,
            };
        }
    }
}
