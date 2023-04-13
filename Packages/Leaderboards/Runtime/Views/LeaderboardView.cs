using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Leaderboards
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text textComponent = default;
        [SerializeField] private Button backButton = default;
        [SerializeField] private Transform participantViewport = default;
        [SerializeField] private ParticipantCardView participantPrefab = default;

        private List<ParticipantCardView> spawned = default;

        public static event UnityAction OnLeaderboardViewClosed;

        private void Awake()
        {
            spawned = new List<ParticipantCardView>();
            participantPrefab.gameObject.SetActive(false);
            backButton.onClick.AddListener(delegate { OnLeaderboardViewClosed?.Invoke(); });
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void Setup(LeaderboardPresenterModel leaderboard, IEnumerable<ParticipantPresenterModel> participants)
        {
            textComponent.text = $"LEADERBOARD: {leaderboard.Name}\nID: {leaderboard.ID}";
            ClearOldParticipants();
            foreach (var p in participants)
            {
                var obj = SpawnParticipant(p);
                spawned.Add(obj);
            }
        }

        public void ClearOldParticipants()
        {
            spawned.ForEach(x => Destroy(x.gameObject));
            spawned.Clear();
        }

        public ParticipantCardView SpawnParticipant(ParticipantPresenterModel participant)
        {
            var p = Instantiate(participantPrefab, participantViewport);
            p.Setup(participant.ID, participant.Name, participant.Score);
            p.gameObject.SetActive(true);
            return p;
        }
    }
}
