using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneLighting 
{
    /// <summary>
    /// Controls the shading style on a camera
    /// NOTE: This is only temporary. The controls can be moved somewhere else.
    /// </summary>
    public class ShadingController : MonoBehaviour
    {
        /// <summary>
        /// A camera component is required to use replacement shaders
        /// </summary>
        private Camera MainCamera;

        /// <summary>
        /// The shader used for replacing
        /// </summary>
        public Shader Shader_Retro;

        /// <summary>
        /// ScriptableObjects used for light settings
        /// </summary>
        public LightingSettings LightingSettings_01;
        public LightingSettings LightingSettings_02;
        public LightingSettings LightingSettings_03;
        public LightingSettings LightingSettings_04;

        private void Start() 
        {
            MainCamera = Camera.main;
        }

        private void Update()
        {
            if (MainCamera == null && Shader_Retro == null) return;

            //Set/Unset replacement shader
            if (Input.GetKeyDown("1"))
            {
                MainCamera.ResetReplacementShader();
            }
            if (Input.GetKeyDown("2"))
            {
                MainCamera.SetReplacementShader(Shader_Retro, "RenderType");
            }

            //Set LightingSettings
            if (Input.GetKeyDown("3"))
            {
                SceneLightingManager.Instance?.ChangeLightSettings(LightingSettings_01);
            }
            if (Input.GetKeyDown("4"))
            {
                SceneLightingManager.Instance?.ChangeLightSettings(LightingSettings_02);
            }
            if (Input.GetKeyDown("5"))
            {
                SceneLightingManager.Instance?.ChangeLightSettings(LightingSettings_03);
            }
            if (Input.GetKeyDown("6"))
            {
                SceneLightingManager.Instance?.ChangeLightSettings(LightingSettings_04);
            }

            //Change scene lighting mode
            if (Input.GetKeyDown("x"))
            {
                SceneLightingManager.Instance?.LerpLightingMode(LightingMode.Default);
            }
            if (Input.GetKeyDown("c"))
            {
                SceneLightingManager.Instance?.LerpLightingMode(LightingMode.Indoor);
            }
        }
    }
}
