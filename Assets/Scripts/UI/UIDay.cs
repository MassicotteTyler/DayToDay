using System;
using Events;
using TMPro;
using UnityEngine;
using World;

namespace UI
{
    /// <summary>
    /// Label for the current day.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIDay : MonoBehaviour
    {
        /// <summary>
        /// The Text component attached to this GameObject.
        /// </summary>
        private TextMeshProUGUI _dayLabel;

        private void Awake()
        {
            _dayLabel = GetComponent<TextMeshProUGUI>();

            UpdateDay();
            EndNodeEvent.OnEndNode += UpdateDay;
        }

        private void OnDestroy()
        {
            EndNodeEvent.OnEndNode -= UpdateDay;
        }

        /// <summary>
        /// Update the day label.
        /// </summary>
        private void UpdateDay()
        {
            var currentDay = WorldManager.Instance.GetGameState().World.Day;
            _dayLabel?.SetText($"Day {currentDay}");
        }
    }
}