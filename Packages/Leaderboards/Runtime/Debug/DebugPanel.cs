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
        [SerializeField] private LeaderboardAPIConfigurationSO configuration = default;
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
            new Vector2(0, -540),
            new Vector2(400, -540),
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
                    logger.LogToUnity($"Request Successful => Leaderboard:\n{leaderboard}");
                    OnLeaderboardCreated?.Invoke(leaderboard);
                };

                leaderboardApi.CreateHighscoreLeaderboard("highscore", 10, onSuccess, PrintFailed);
            });
            CreateButton("Create Incremental Leaderboard", delegate
            {
                UnityAction<Leaderboard> onSuccess = (leaderboard) =>
                {
                    logger.LogToUnity($"Request Successful => Leaderboard:\n{leaderboard}");
                    OnLeaderboardCreated?.Invoke(leaderboard);
                };

                leaderboardApi.CreateIncrementalLeaderboard("incremental", 10, onSuccess, PrintFailed);
            });
            CreateButtonWithInput("Get Leaderboard", (input) =>
            {
                UnityAction<Leaderboard> onSuccess = (leaderboard) =>
                {
                    logger.LogToUnity($"Request Successful => Leaderboard:\n{leaderboard}");
                    OnLeaderboardFetched?.Invoke(new Leaderboard[] { leaderboard });
                };

                leaderboardApi.GetLeaderboard(Guid.Parse(input), onSuccess, PrintFailed);
            });
            //CreateButton("Get Highscore Leaderboards", delegate
            //{
            //    UnityAction<Leaderboard[]> onSuccess = (leaderboards) =>
            //    {
            //        logger.LogToUnity($"Request Successful => Leaderboards:");
            //        foreach (var lb in leaderboards)
            //        {
            //            logger.LogToUnity($"{lb}");
            //        }
            //        OnLeaderboardFetched?.Invoke(leaderboards);
            //    };

            //    leaderboardApi.GetHighscoreLeaderboards(onSuccess, PrintFailed);
            //});
            //CreateButton("Get Incremental Leaderboards", delegate
            //{
            //    UnityAction<Leaderboard[]> onSuccess = (leaderboards) =>
            //    {
            //        logger.LogToUnity($"Request Successful => Leaderboards:");
            //        foreach (var lb in leaderboards)
            //        {
            //            logger.LogToUnity($"{lb}");
            //        }
            //        OnLeaderboardFetched?.Invoke(leaderboards);
            //    };

            //    leaderboardApi.GetIncrementalLeaderboards(onSuccess, PrintFailed);
            //});
            CreateButton("Get All Leaderboards", delegate
            {
                UnityAction<Leaderboard[]> onSuccess = (leaderboards) =>
                {
                    logger.LogToUnity($"Request Successful => Leaderboards:");
                    foreach (var lb in leaderboards)
                    {
                        logger.LogToUnity($"{lb}");
                    }
                    OnLeaderboardFetched?.Invoke(leaderboards);
                };

                leaderboardApi.GetAllLeaderboards(onSuccess, PrintFailed);
            });
            CreateButtonWithInput("Create Participant", (input) =>
            {
                UnityAction<LeaderboardParticipant> onSuccess = (participant) =>
                {
                    logger.LogToUnity($"Request Successful => Participant: {participant}");
                };

                leaderboardApi.CreateLeaderboardParticipant(Guid.Parse(input), Guid.NewGuid().ToString(), "Bob", onSuccess, PrintFailed);
            });
            CreateButtonWithInput("Create Participant (M)", (input) =>
            {
                UnityAction<LeaderboardParticipant> onSuccess = (participant) =>
                {
                    logger.LogToUnity($"Request Successful => Participant: {participant}");
                };

                leaderboardApi.CreateLeaderboardParticipantWithMetadata(Guid.Parse(input), Guid.NewGuid().ToString(), "Bob", 
                    new Dictionary<string, string>() { { "emblem", "t_emblem_01"} },onSuccess, PrintFailed);
            });
            CreateButtonWith2Inputs("Update Score", (input1, input2) =>
            {
                UnityAction<LeaderboardParticipant> onSuccess = (participant) =>
                {
                    logger.LogToUnity($"Request Successful => Participant: {participant}");
                };

                leaderboardApi.UpdateParticipantScore(Guid.Parse(input1), input2, 10, onSuccess, PrintFailed);
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

        private void CreateButtonWith2Inputs(string buttonLabel, UnityAction<string, string> onClick)
        {
            var input1 = CreateInputField();
            var input2 = CreateInputField();
            CreateButton(buttonLabel, delegate
            {
                onClick?.Invoke(input1.text, input2.text);
            });
        }

        private void CreateButtonWith3Inputs(string buttonLabel, UnityAction<string, string, string> onClick)
        {
            var input1 = CreateInputField();
            var input2 = CreateInputField();
            var input3 = CreateInputField();
            CreateButton(buttonLabel, delegate
            {
                onClick?.Invoke(input1.text, input2.text, input3.text);
            });
        }

        private void CreateLeaderboardAPI()
        {
            logger = new DefaultLogger();
            leaderboardApi = new LeaderboardAPI(logger, configuration);
        }

        private void PrintFailed(string message)
        {
            UnityEngine.Debug.LogError("Request Failed => " + message);
        }
    }
}