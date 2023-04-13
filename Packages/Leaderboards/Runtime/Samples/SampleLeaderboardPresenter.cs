using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leaderboards;
using Leaderboards.Debug;
using UnityEngine;
using UnityEngine.Events;

namespace Leaderboards.Samples
{
    public class SampleLeaderboardPresenter : MonoBehaviour
    {
        [SerializeField] private LeaderboardsView leaderboardsView = default;
        [SerializeField] private LeaderboardView leaderboardView = default;

        private ILogger logger = default;
        private LeaderboardAPI leaderboardApi = default;

        private void Awake()
        {
            CreateLeaderboardAPI();
            leaderboardView.SetActive(false);
            leaderboardsView.SetActive(true);
        }

        private void OnEnable()
        {
            DebugPanel.OnLeaderboardFetched += SpawnLeaderboards;
            LeaderboardCardView.OnLeaderboardSelected += SelectLeaderboard;
            LeaderboardView.OnLeaderboardViewClosed += ShowLeaderboardsView;
        }

        private void CreateLeaderboardAPI()
        {
            logger = new DefaultLogger();
            leaderboardApi = new LeaderboardAPI(logger, new UnityWebRequestAPI.Config
            {
                BaseUrl = "http://localhost:8080",
                Subdomain = "default",
                ApiKey = "user",
                Version = 1,
            });
        }

        private void SpawnLeaderboards(Leaderboard[] leaderboards)
        {
            var models = leaderboards.Select(x => new LeaderboardPresenterModel
            {
                ID = x.ID,
                Name = x.Name,
                Capacity = x.Capacity,
            });

            leaderboardsView.Setup(models.ToArray());
        }

        private void SelectLeaderboard(Guid leaderboardId)
        {
            UnityAction<Leaderboard> onSuccess = (lb) =>
            {
                leaderboardView.SetActive(true);
                leaderboardsView.SetActive(false);

                UnityAction<LeaderboardParticipant[]> onSuccess2 = (participants) =>
                {
                    var lbModel = new LeaderboardPresenterModel
                    {
                        ID = lb.ID,
                        Name = lb.Name,
                        Capacity = lb.Capacity,
                    };

                    var partModels = participants.Select(x => new ParticipantPresenterModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Score = x.Score,
                    });

                    leaderboardView.Setup(lbModel, partModels.ToArray());
                };

                leaderboardApi.GetLeaderboardParticipants(leaderboardId, onSuccess2, delegate { logger.LogErrorToUnity("get leaderboard participants failed"); });
            };

            leaderboardApi.GetLeaderboard(leaderboardId, onSuccess, delegate { logger.LogErrorToUnity("get leaderboard failed"); });
        }

        private void ShowLeaderboardsView()
        {
            leaderboardView.SetActive(false);
            leaderboardsView.SetActive(true);
        }

        private void OnDisable()
        {
            DebugPanel.OnLeaderboardFetched -= SpawnLeaderboards;
            LeaderboardCardView.OnLeaderboardSelected -= SelectLeaderboard;
            LeaderboardView.OnLeaderboardViewClosed -= ShowLeaderboardsView;
        }
    }
}