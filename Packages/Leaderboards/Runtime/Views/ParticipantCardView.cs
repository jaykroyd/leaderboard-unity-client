﻿using System;
using TMPro;
using UnityEngine;

namespace Leaderboards
{
    public class ParticipantCardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleComponent = default;

        public void Setup(string id, string name, long score, string metadata)
        {
            titleComponent.text = $"ID: {id}\nName: {name}\nScore: {score}\nMetadata: {metadata}";
        }
    }
}
