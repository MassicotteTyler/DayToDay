using System;
using System.Collections;
using UnityEngine;

namespace SceneLighting
{
    /// <summary>
    /// Values used for scene lighting are stored here
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "NewLightingSettings", menuName = "ScriptableObjects/LightingSettings", order = 2)]
    public class LightingSettings : ScriptableObject
    {
        public bool UseCustomShader;
        public Shader Shader_Retro;

        public Material Skybox_Material;

        [ColorUsage(false, true)]
        public Color Ambient_Colour = Color.white;
        public float Ambient_Intensity_Indoor;

        public bool Fog_Enabled = true;
        public Color Fog_Colour = Color.white;
        public float Fog_Start = 10.0f;
        public float Fog_End = 50.0f;
    }
}
