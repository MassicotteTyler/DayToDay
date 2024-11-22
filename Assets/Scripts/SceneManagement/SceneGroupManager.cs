using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    /// <summary>
    /// Manages the loading and unloading of scene groups.
    /// </summary>
    public class SceneGroupManager
    {
        /// <summary>
        /// Event triggered when a scene is loaded.
        /// </summary>
        public event Action<string> OnSceneLoad = delegate { };

        /// <summary>
        /// Event triggered when a scene is unloaded.
        /// </summary>
        public event Action<string> OnSceneUnload = delegate { };

        /// <summary>
        /// Event triggered when a scene group is fully loaded.
        /// </summary>
        public static event Action<SceneGroup> OnSceneGroupLoaded = delegate { };

        private SceneGroup ActiveSceneGroup;

        /// <summary>
        /// Loads the scenes in the specified scene group.
        /// </summary>
        /// <param name="group">The scene group to load.</param>
        /// <param name="progress">Progress reporter for the loading process.</param>
        /// <param name="reloadDupScenes">If set to <c>true</c>, duplicate scenes will be reloaded.</param>
        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
        {
            ActiveSceneGroup = group;
            var loadedScenes = new List<string>();

            // TODO: Should scenes unload before or after?
            await UnloadScenes();
            var sceneCount = SceneManager.sceneCount;
            for (var index = 0; index < sceneCount; index++)
            {
                var scene = SceneManager.GetSceneAt(index);
                if (scene.name == "DontDestroyOnLoad") continue;
                loadedScenes.Add(scene.name);
            }

            var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (var index = 0; index < totalScenesToLoad; index++)
            {
                var sceneData = group.Scenes[index];
                if (reloadDupScenes == false && loadedScenes.Contains(sceneData.Name)) continue;

                var operation = SceneManager.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
                operationGroup.Operations.Add(operation);

                OnSceneLoad?.Invoke(sceneData.Name);
            }

            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }

            Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            // await UnloadScenes();
            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }

            OnSceneGroupLoaded?.Invoke(ActiveSceneGroup);
        }

        /// <summary>
        /// Unloads all currently loaded scenes except the active scene and the boot scene.
        /// </summary>
        public async Task UnloadScenes()
        {
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;

            int sceneCount = SceneManager.sceneCount;
            for (var index = sceneCount - 1; index > 0; index--)
            {
                var sceneAt = SceneManager.GetSceneAt(index);
                if (!sceneAt.isLoaded)
                {
                    Debug.Log($"Scene not loaded: {sceneAt.name}");
                    continue;
                }

                var sceneName = sceneAt.name;
                // TODO: Use static string for bootstrapper scene name
                if (sceneName.Equals(Bootstrapper.BootSceneName)) continue;
                scenes.Add(sceneName);
            }

            // Create an AsyncOperationGroup
            var operationGroup = new AsyncOperationGroup(scenes.Count);
            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null)
                {
                    Debug.LogError($"Failed to unload scene: {scene}");
                    continue;
                
                }

                operationGroup.Operations.Add(operation);
                OnSceneUnload?.Invoke(scene);
            }

            // Wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }

            // Optional: Unload unused assets from memory
            // Resources.UnloadUnusedAssets();
        }
    }

    /// <summary>
    /// Represents a group of asynchronous operations.
    /// </summary>
    public readonly struct AsyncOperationGroup
    {
        /// <summary>
        /// The list of asynchronous operations.
        /// </summary>
        public readonly List<AsyncOperation> Operations;

        /// <summary>
        /// Gets the average progress of all operations.
        /// </summary>
        public float Progress => Operations?.Average(o => o.progress) ?? 0;

        /// <summary>
        /// Gets a value indicating whether all operations are done.
        /// </summary>
        public bool IsDone => Operations?.All(o => o.isDone) ?? true;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncOperationGroup"/> struct.
        /// </summary>
        /// <param name="initialCapacity">The initial capacity of the operations list.</param>
        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }
}