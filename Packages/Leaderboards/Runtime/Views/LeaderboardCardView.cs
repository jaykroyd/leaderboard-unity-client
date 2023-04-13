using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Leaderboards
{
    public class LeaderboardCardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleComponent = default;
        [SerializeField] private Button buttonComponent = default;
        private Guid leaderboardId = default;

        public static event UnityAction<Guid> OnLeaderboardSelected;

        private void Awake()
        {
            buttonComponent.onClick.AddListener(OnClick);
        }

        public void Setup(Guid id, string name, long capacity)
        {
            this.leaderboardId = id;
            titleComponent.text = $"ID: {id}\nName: {name}\nCapacity: {capacity}";
        }

        private void OnClick()
        {
            OnLeaderboardSelected?.Invoke(leaderboardId);
        }
    }
}
