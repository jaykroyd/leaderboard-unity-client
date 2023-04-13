using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Leaderboards.Debug
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] private Button buttonTemplate = default;
        [SerializeField] private TMP_InputField inputTemplate = default;
        [SerializeField] private LayoutGroup buttonsParent = default;
        [SerializeField] private Button collapseButton = default;
        [SerializeField] private RectTransform collapsablePanel = default;
        [SerializeField] private Image collapseIcon = default;

        private ILogger logger = default;
        private LeaderboardAPI leaderboardApi = default;

        public static event UnityAction<Leaderboard> OnLeaderboardCreated;
        public static event UnityAction<Leaderboard[]> OnLeaderboardFetched;

        private Vector2[] windowPositions = new Vector2[]
        {
            new Vector2(0, -300),
            new Vector2(400, -300),
        };

        private void Awake()
        {
            CreateLeaderboardAPI();
            CreateDebugButtons();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void CreateDebugButtons()
        {
            collapseButton.onClick.AddListener(ToggleCollapse);
            SetCollapsePosition(windowPositions[0]);

            buttonTemplate.gameObject.SetActive(false);
            inputTemplate.gameObject.SetActive(false);

            CreateButton("Create Highscore Leaderboard", delegate
            {
                UnityAction<Leaderboard> onSuccess = (leaderboard) =>
                {
                    logger.LogToUnity($"Request Successfull => Leaderboard:\n{leaderboard}");
                    OnLeaderboardCreated?.Invoke(leaderboard);
                };

                leaderboardApi.CreateHighscoreLeaderboard("highscore", 100, onSuccess, PrintFailed);
            });
            CreateButton("Create Incremental Leaderboard", delegate
            {
                UnityAction<Leaderboard> onSuccess = (leaderboard) =>
                {
                    logger.LogToUnity($"Request Successfull => Leaderboard:\n{leaderboard}");
                    OnLeaderboardCreated?.Invoke(leaderboard);
                };

                leaderboardApi.CreateIncrementalLeaderboard("incremental", 100, onSuccess, PrintFailed);
            });
            CreateButtonWithInput("Get Leaderboard", (input) =>
            {
                UnityAction<Leaderboard> onSuccess = (leaderboard) =>
                {
                    logger.LogToUnity($"Request Successfull => Leaderboard:\n{leaderboard}");
                    OnLeaderboardFetched?.Invoke(new Leaderboard[] { leaderboard });
                };

                leaderboardApi.GetLeaderboard(Guid.Parse(input), onSuccess, PrintFailed);
            });
            CreateButton("Get Highscore Leaderboards", delegate
            {
                UnityAction<Leaderboard[]> onSuccess = (leaderboards) =>
                {
                    logger.LogToUnity($"Request Successfull => Leaderboards:");
                    foreach (var lb in leaderboards)
                    {
                        logger.LogToUnity($"{lb}");
                    }
                    OnLeaderboardFetched?.Invoke(leaderboards);
                };

                leaderboardApi.GetHighscoreLeaderboards(onSuccess, PrintFailed);
            });
            CreateButton("Get Incremental Leaderboards", delegate
            {
                UnityAction<Leaderboard[]> onSuccess = (leaderboards) =>
                {
                    logger.LogToUnity($"Request Successfull => Leaderboards:");
                    foreach (var lb in leaderboards)
                    {
                        logger.LogToUnity($"{lb}");
                    }
                    OnLeaderboardFetched?.Invoke(leaderboards);
                };

                leaderboardApi.GetIncrementalLeaderboards(onSuccess, PrintFailed);
            });
            CreateButton("Get All Leaderboards", delegate
            {
                UnityAction<Leaderboard[]> onSuccess = (leaderboards) =>
                {
                    logger.LogToUnity($"Request Successfull => Leaderboards:");
                    foreach (var lb in leaderboards)
                    {
                        logger.LogToUnity($"{lb}");
                    }
                    OnLeaderboardFetched?.Invoke(leaderboards);
                };

                leaderboardApi.GetAllLeaderboards(onSuccess, PrintFailed);
            });
            CreateButtonWithInput("Create Leaderboard Participant", (input) =>
            {
                UnityAction<LeaderboardParticipant> onSuccess = (participant) =>
                {
                    logger.LogToUnity($"Request Successfull => Participant: {participant}");
                };

                // leaderboardApi.CreateLeaderboardParticipant(new LeaderboardParticipant { }, onSuccess, PrintFailed);
            });
        }

        private void ToggleCollapse()
        {
            int currentPosIndex = Array.IndexOf(windowPositions, collapsablePanel.anchoredPosition);
            int nextPosIndex = (currentPosIndex + 1) % windowPositions.Length;
            SetCollapsePosition(windowPositions[nextPosIndex]);
        }

        private void SetCollapsePosition(Vector2 pos)
        {
            collapsablePanel.anchoredPosition = pos;
            collapseIcon.transform.rotation = collapsablePanel.anchoredPosition == windowPositions[1]
                ? Quaternion.Euler(0, 0, -90)
                : Quaternion.Euler(0, 0, 90);
        }

        private void CreateButton(string label, UnityAction onClick)
        {
            var button = GameObject.Instantiate(buttonTemplate, buttonsParent.transform);
            button.GetComponentInChildren<TMP_Text>(true).text = label;
            button.onClick.AddListener(onClick);
            button.gameObject.SetActive(true);
        }

        private TMP_InputField CreateInputField()
        {
            var input = GameObject.Instantiate(inputTemplate, buttonsParent.transform);
            input.gameObject.SetActive(true);
            return input;
        }

        private void CreateButtonWithInput(string buttonLabel, UnityAction<string> onClick)
        {
            var input = CreateInputField();
            CreateButton(buttonLabel, delegate
            {
                onClick?.Invoke(input.text);
            });
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

        private void PrintFailed(string message)
        {
            UnityEngine.Debug.LogError("Request Failed => " + message);
        }
    }
}