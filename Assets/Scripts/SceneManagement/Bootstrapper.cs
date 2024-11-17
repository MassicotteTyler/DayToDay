using System;
using DevTools.Node;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace SceneManagement
{
    /// <summary>
    /// Manages the initialization and loading of the boot scene.
    /// </summary>
    public class Bootstrapper : Singleton<Bootstrapper>
    {
        /// <summary>
        /// The name of the boot scene.
        /// </summary>
        public static string BootSceneName = "Boot";

        /// <summary>
        /// The scene loader instance.
        /// </summary>
        public SceneLoader SceneLoader { get; private set; }
       
        /// <summary>
        /// Updates the scene loader instance.
        /// </summary>
        /// <param name="sceneLoader">Scene load instance to use</param>
        public void UpdateSceneLoader(SceneLoader sceneLoader)
        {
            SceneLoader = sceneLoader;
        }

        public void Update()
        {
            // TODO: Replace this with an action later
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneLoader.ReloadActiveSceneGroup();
            }
            #endif
        }

        /// <summary>
        /// Initializes the bootstrapper before any scene is loaded.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static async void Init()
        {
            Debug.Log("Boostrapper...");

            // Ensure Boot scene exists
            var bootScenePath = SceneUtility.GetScenePathByBuildIndex(0);
            if (string.IsNullOrWhiteSpace(bootScenePath))
            {
                Debug.LogError("Boot scene not found. Ensure a scene is added to the build settings at index 0.");
                return;
            }
            BootSceneName = SceneManager.GetSceneByBuildIndex(0).name;
            
            #if UNITY_EDITOR
            if (!NodeManagementTool.config.enableBootstrapper)
            {
                return;
            }
            #endif
            
            SceneManager.LoadSceneAsync(sceneBuildIndex: 0, LoadSceneMode.Single);
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void AfterSceneLoad()
        {
            #if UNITY_EDITOR
            if (NodeManagementTool.config.enableBootstrapper) return;
            
            // Ensure the UI transitions out if we're not using the bootstrapper events
            UIManager.Instance.OnNodeTransitionEnd?.Invoke();
            
            if (NodeManagementTool.config.playerPrefab == null) return;
            
            // check if a player prefab already exists
            var player = GameObject.FindWithTag("Player");
            if (player) return;
                    
                    
            // Spawn player prefab
            player = Instantiate(NodeManagementTool.config.playerPrefab,
                NodeManagementTool.config.playerSpawnPosition,
                Quaternion.identity * NodeManagementTool.config.playerSpawnRotation);
                    
            // Update camera rotation
            var playerCamera = player.GetComponentInChildren<Camera>();
            if (!playerCamera) return;
            playerCamera.transform.localRotation = NodeManagementTool.config.playerSpawnRotation;
            #endif
        }
    }
}