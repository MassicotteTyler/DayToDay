using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using UnityEngine;

namespace SceneManagement
{
    /// <summary>
    /// Represents a group of scenes.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SceneGroup", order = 1)]
    public class SceneGroup : ScriptableObject
    {
        /// <summary>
        /// The name of the scene group.
        /// </summary>
        public string GroupName = "New Scene Group";

        /// <summary>
        /// The list of scenes in the group.
        /// </summary>
        public List<SceneData> Scenes;

        /// <summary>
        /// Finds the name of a scene by its type.
        /// </summary>
        /// <param name="sceneType">The type of the scene.</param>
        /// <returns>The name of the scene if found; otherwise, null.</returns>
        public string FindSceneNameByType(SceneType sceneType)
        {
            return Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Name;
        }
    }

    /// <summary>
    /// Represents data for a single scene.
    /// </summary>
    [Serializable]
    public class SceneData
    {
        /// <summary>
        /// The reference to the scene.
        /// </summary>
        public SceneReference Reference;

        /// <summary>
        /// The name of the scene.
        /// </summary>
        public string Name => Reference.Name;

        /// <summary>
        /// The type of the scene.
        /// </summary>
        public SceneType SceneType;
    }

    /// <summary>
    /// Enum representing different types of scenes.
    /// </summary>
    public enum SceneType
    {
        /// <summary>
        /// The active scene.
        /// </summary>
        ActiveScene,

        /// <summary>
        /// The main menu scene.
        /// </summary>
        MainMenu,

        /// <summary>
        /// The user interface scene.
        /// </summary>
        UserInterface,

        /// <summary>
        /// The HUD scene.
        /// </summary>
        HUD,

        /// <summary>
        /// The cinematic scene.
        /// </summary>
        Cinematic,

        /// <summary>
        /// The environment scene.
        /// </summary>
        Environment,

        /// <summary>
        /// The tooling scene.
        /// </summary>
        Tooling
    }
}