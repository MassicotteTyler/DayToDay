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

            SceneManager.LoadSceneAsync(sceneBuildIndex: 0, LoadSceneMode.Single);
            BootSceneName = SceneManager.GetSceneByBuildIndex(0).name;
        }
    }
}