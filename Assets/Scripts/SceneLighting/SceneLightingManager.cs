using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using Utility;

namespace SceneLighting
{
    /// <summary>
    /// Possible lighting modes.
    /// Used to determine ambient light intensity value.
    /// </summary>
    public enum LightingMode
    {
        Default, 
        Indoor
    }

    /// <summary>
    /// Singleton that controls the current scene's lighting parameters
    /// </summary>
    public class SceneLightingManager : Singleton<SceneLightingManager>
    {
        /// <summary>
        /// Store the last set LightingSettings
        /// </summary>
        private LightingSettings _currentSettings;

        /// <summary>
        /// The current intensity of the ambient lighting
        /// This effects the brightness of the scene
        /// </summary>
        public float _ambientIntensity { get; private set; }

        /// <summary>
        /// Current lighting mode
        /// </summary>
        public LightingMode _lightingMode { get; private set; }

        /// <summary>
        /// flat rate in which intensity changes
        /// </summary>
        private float _intensityStep = 1.5f;

        /// <summary>
        /// Manage the ambient intensity lerp routine
        /// </summary>
        private Coroutine _intensityLerpRoutine;

        /// <summary>
        /// Set new LightingSettings
        /// </summary>
        /// <param name="settingSO">new settings</param>
        public void ChangeLightSettings(LightingSettings settingsSO)
        {
            if (!settingsSO) return;

            //Stop any running coroutines
            if(_intensityLerpRoutine != null)
            {
                StopCoroutine(_intensityLerpRoutine);
                _intensityLerpRoutine = null;
            }

            _currentSettings = settingsSO;

            //Not sure how RenderSettings work with additive scenes...
            //Skybox settings
            RenderSettings.skybox = settingsSO.Skybox_Material;

            //Ambient light settings
            RenderSettings.ambientLight = settingsSO.Ambient_Colour * Mathf.Pow(2.0f, GetIntensity(_lightingMode));
            _ambientIntensity = GetIntensity(_lightingMode);

            //Fog settings
            RenderSettings.fog = settingsSO.Fog_Enabled;
            RenderSettings.fogColor = settingsSO.Fog_Colour;
            RenderSettings.fogStartDistance = settingsSO.Fog_Start;
            RenderSettings.fogEndDistance = settingsSO.Fog_End;
        }

        private void Awake()
        {
           LightingEvent.OnLightingEvent += ChangeLightSettings; 
           LightingModeEvent.OnLightingIntensityEvent += LerpLightingMode;
        }
        
        private void OnDestroy()
        {
            LightingEvent.OnLightingEvent -= ChangeLightSettings;
            LightingModeEvent.OnLightingIntensityEvent -= LerpLightingMode;
        }

        /// <summary>
        /// Set the ambient light intensity of the scene.
        /// Use this to instantly set ambient intensity.
        /// </summary>
        /// <param name="intensity">Intensity level to set</param>
        public void SetAmbientIntensity(float intensity = 0.0f)
        {
            if (_currentSettings == null) return;

            RenderSettings.ambientLight = _currentSettings.Ambient_Colour * Mathf.Pow(2.0f, intensity);
            _ambientIntensity = intensity;
        }

        /// <summary>
        /// Change the ambient light intensity of the scene with LightingMode
        /// Use this to lerp light levels with LightingMode
        /// </summary>
        /// <param name="mode">Target lighting mode</param>
        public void LerpLightingMode(LightingMode mode)
        {
            //Do not do anything if we are switching to the same mode
            if (_lightingMode == mode) return;

            _lightingMode = mode;

            //Ensures there is only one coroutine running.
            //Override the last coroutine
            if (_intensityLerpRoutine != null)
            {
                StopCoroutine(_intensityLerpRoutine);
            }

            _intensityLerpRoutine = StartCoroutine(LerpAmbientIntensity(GetIntensity(mode)));
        }


        /// <summary>
        /// Coroutine to lerp ambient colour intensity
        /// </summary>
        /// <param name="target">Target intensity</param>
        IEnumerator LerpAmbientIntensity(float target)
        {
            if (_currentSettings == null) yield break;

            //increase or decrease intensity
            int direction = _ambientIntensity < target ? 1 : -1;

            //Determine the smaller number for clamping later
            float min = _ambientIntensity < target ? _ambientIntensity : target;
            float max = _ambientIntensity < target ? target : _ambientIntensity;

            //as long as the target is not reached
            while (_ambientIntensity != target)
            {
                _ambientIntensity = Mathf.Clamp(_ambientIntensity + (_intensityStep * direction * Time.deltaTime), min, max);

                RenderSettings.ambientLight = _currentSettings.Ambient_Colour * Mathf.Pow(2.0f, _ambientIntensity);

                yield return null;
            }

            _intensityLerpRoutine = null;
        }

        /// <summary>
        /// Get the intensity level for the current light settings
        /// </summary>
        /// <param name="mode">Target lighting mode</param>
        /// <returns>Intensity value</returns>
        float GetIntensity(LightingMode mode)
        {
            //Default value if no settings
            if (_currentSettings == null) return 0.0f;

            float intensity = 0.0f;

            switch (mode)
            {
                case LightingMode.Indoor:
                    intensity = _currentSettings.Ambient_Intensity_Indoor;
                    break;
                default:
                    break;
            }

            return intensity;
        }
    }
}
