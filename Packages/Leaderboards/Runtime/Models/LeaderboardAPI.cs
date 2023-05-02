using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Leaderboards
{
    public class LeaderboardAPI : UnityWebRequestAPI, ILeaderboardCreator, ILeaderboardProvider, ILeaderboardParticipantsProvider
    {
        private const int LEADERBOARD_MODE_HIGHSCORE = 0;
        private const int LEADERBOARD_MODE_INCREMENTAL = 1;

        public new struct Config : ILeaderboardAPIConfiguration
        {
            public string ApiKey;
            public string BaseUrl;
            public string Subdomain;
            public int Version;

            string IUnityWebRequestConfiguration.ApiKey => ApiKey;
            string IUnityWebRequestConfiguration.BaseUrl => BaseUrl;
            string IUnityWebRequestConfiguration.Subdomain => Subdomain;
            int IUnityWebRequestConfiguration.Version => Version;
        }

        public LeaderboardAPI(ILogger logger, ILeaderboardAPIConfiguration config) : base(logger, config)
        {

        }

        #region GET_LEADERBOARDS
        public void GetLeaderboard(Guid id, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed)
        {
            GET<GetLeaderboardResponse>(
                $"/leaderboards/{id}",
                (resp) => { onSuccess(resp.leaderboard.ToLeaderboard()); },
                onFailed
            );
        }

        public void GetHighscoreLeaderboards(UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed)
        {
            GetLeaderboards(LEADERBOARD_MODE_HIGHSCORE, onSuccess, onFailed);
        }

        public void GetIncrementalLeaderboards(UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed)
        {
            GetLeaderboards(LEADERBOARD_MODE_INCREMENTAL, onSuccess, onFailed);
        }

        public void GetAllLeaderboards(UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed)
        {
            GetLeaderboards(onSuccess, onFailed);
        }

        private void GetLeaderboards(UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed)
        {
            GetLeaderboards($"/leaderboards", onSuccess, onFailed);
        }

        private void GetLeaderboards(int mode, UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed)
        {
            GetLeaderboards($"/leaderboards?mode={mode}", onSuccess, onFailed);
        }

        private void GetLeaderboards(string endpoint, UnityAction<Leaderboard[]> onSuccess, UnityAction<string> onFailed)
        {
            GET<GetLeaderboardsResponse>(
                endpoint,
                (resp) => { onSuccess(resp.leaderboards.Select(x => x.ToLeaderboard()).ToArray()); },
                onFailed
            );
        }
        #endregion

        #region CREATE_LEADERBOARDS
        public void CreateHighscoreLeaderboard(string name, int capacity, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed)
        {
            var req = new CreateLeaderboardRequest
            {
                name = name,
                capacity = capacity,
                mode = LEADERBOARD_MODE_HIGHSCORE,
            };

            CreateLeaderboard(req, onSuccess, onFailed);
        }

        public void CreateIncrementalLeaderboard(string name, int capacity, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed)
        {
            var req = new CreateLeaderboardRequest
            {
                name = name,
                capacity = capacity,
                mode = LEADERBOARD_MODE_INCREMENTAL,
            };

            CreateLeaderboard(req, onSuccess, onFailed);
        }

        private void CreateLeaderboard(CreateLeaderboardRequest req, UnityAction<Leaderboard> onSuccess, UnityAction<string> onFailed)
        {
            POST<CreateLeaderboardRequest, CreateLeaderboardResponse>(
                "/leaderboards",
                req,
                (resp) => { onSuccess(resp.leaderboard.ToLeaderboard()); },
                onFailed
            );
        }
        #endregion

        #region PARTICIPANTS
        public void GetLeaderboardParticipants(Guid leaderboardID, UnityAction<LeaderboardParticipant[]> onSuccess, UnityAction<string> onFailed)
        {
            GET<GetLeaderboardParticipantsResponse>(
                $"/leaderboards/{leaderboardID}/participants",
                (resp) => { onSuccess(resp.participants.Select(x => x.ToLeaderboardParticipant()).ToArray()); },
                onFailed
            );
        }

        public void CreateLeaderboardParticipant(Guid leaderboardID, string participantID, string participantName, UnityAction<LeaderboardParticipant> onSuccess, UnityAction<string> onFailed)
        {
            CreateLeaderboardParticipantWithMetadata(leaderboardID, participantID, participantName, new Dictionary<string, string>(), onSuccess, onFailed);
        }

        public void CreateLeaderboardParticipantWithMetadata(Guid leaderboardID, string participantID, string participantName, Dictionary<string, string> metadata, UnityAction<LeaderboardParticipant> onSuccess, UnityAction<string> onFailed)
        {
            var req = new CreateParticipantRequest
            {
                leaderboard_id = leaderboardID.ToString(),
                external_id = participantID,
                name = participantName,
                metadata = metadata,
            };

            POST<CreateParticipantRequest, CreateParticipantResponse>(
                "/participants",
                req,
                (resp) => { onSuccess(resp.participant.ToLeaderboardParticipant()); },
                onFailed
            );
        }
        #endregion
    }
}
