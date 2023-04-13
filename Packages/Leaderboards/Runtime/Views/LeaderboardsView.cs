using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leaderboards
{
    public class LeaderboardsView : MonoBehaviour
    {
        [SerializeField] private Transform leaderboardViewport = default;
        [SerializeField] private LeaderboardCardView leaderboardPrefab = default;

        private List<LeaderboardCardView> spawned = default;

        private void Awake()
        {
            spawned = new List<LeaderboardCardView>();
            leaderboardPrefab.gameObject.SetActive(false);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void Setup(IEnumerable<LeaderboardPresenterModel> leaderboards)
        {
            ClearOldLeaderboards();
            foreach (var lb in leaderboards)
            {
                var obj = SpawnLeaderboard(lb);
                spawned.Add(obj);
            }
        }

        public void ClearOldLeaderboards()
        {
            spawned.ForEach(x => Destroy(x.gameObject));
            spawned.Clear();
        }

        public LeaderboardCardView SpawnLeaderboard(LeaderboardPresenterModel leaderboard)
        {
            var lb = Instantiate(leaderboardPrefab, leaderboardViewport);
            lb.Setup(leaderboard.ID, leaderboard.Name, leaderboard.Capacity);
            lb.gameObject.SetActive(true);
            return lb;
        }
    }
}
