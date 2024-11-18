using System;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Event for when pills are consumed.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Pills Consumed", order = 3)]
    public class ConsumedPillsEvent : GameEvent
    {
        /// <summary>
        /// Action to be invoked when pills are consumed.
        /// </summary>
        public static Action onPillsConsumed;

        public override void Invoke(GameObject invoker = null)
        {
            onPillsConsumed?.Invoke();
        }
    }
}