using UnityEngine;

namespace Leaderboards
{
    [CreateAssetMenu(fileName = "LeaderboardAPIConfigurationSO", menuName = "Leaderboards/Configuration")]
    public class LeaderboardAPIConfigurationSO : ScriptableObject, ILeaderboardAPIConfiguration
    {
        [SerializeField] private string apiKey = default;
        [SerializeField] private string baseUrl = default;
        [SerializeField] private string subdomain = default;
        [SerializeField] private int version = default;

        public string ApiKey => apiKey;
        public string BaseUrl => baseUrl;
        public string Subdomain => subdomain;
        public int Version => version;
    }
}
