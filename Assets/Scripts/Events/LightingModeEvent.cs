using UnityEngine;
using System;
using SceneLighting;

namespace Events
{
    /// <summary>
    /// Event for when lighting mode is changed.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Lighting Mode", order = 5)]
    public class LightingModeEvent : GameEvent
    {
        /// <summary>
        /// Action to be invoked when lighting mode is changed.
        /// </summary>
        public static Action<LightingMode> OnLightingIntensityEvent;

        /// <summary>
        /// Lighting mode to be applied.
        /// </summary>
        [SerializeField]
        private LightingMode mode;

        protected override void Execute(GameObject invoker = null)
        {
            base.Execute();
            OnLightingIntensityEvent?.Invoke(mode);
        }
    }
}
