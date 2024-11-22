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
        
        public override void Invoke(GameObject invoker = null)
        {
            base.Invoke();
            OnLightingEvent?.Invoke(settings);
        }
    }
    
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
        
        public override void Invoke(GameObject invoker = null)
        {
            base.Invoke();
            OnLightingIntensityEvent?.Invoke(mode);
        }
    }
}