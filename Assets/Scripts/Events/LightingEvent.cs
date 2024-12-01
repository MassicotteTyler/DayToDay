using System;
using SceneLighting;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Event for when lighting settings are changed.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Lighting", order = 4)]
    public class LightingEvent : GameEvent
    {
        /// <summary>
        /// Action to be invoked when lighting event is triggered.
        /// </summary>
        public static Action<SceneLighting.LightingSettings> OnLightingEvent;

        /// <summary>
        /// Lighting settings to be applied.
        /// </summary>
        [SerializeField]
        private SceneLighting.LightingSettings settings;
        
        protected override void Execute(GameObject invoker = null)
        {
            base.Execute();
            OnLightingEvent?.Invoke(settings);
        }
    }
}