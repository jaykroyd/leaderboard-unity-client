using System;
using UnityEngine.Events;

namespace Leaderboards
{
    public interface ILeaderboardParticipantsProvider
    {
        void GetLeaderboardParticipants(Guid id, UnityAction<LeaderboardParticipant[]> onSuccess, UnityAction<string> onFailed);
    }
}
