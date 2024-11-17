using System;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement
{
    /// <summary>
    /// Manages the loading of scenes and updates the loading UI.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// The loading bar image.
        /// </summary>
        [SerializeField] private Image loadingBar;

        /// <summary>
        /// The speed at which the loading bar fills.
        /// </summary>
        [SerializeField] private float fillSpeed = 0.5f;

        /// <summary>
        /// The canvas displaying the loading screen.
        /// </summary>
        [SerializeField] private Canvas loadingCanvas;

        /// <summary>
        /// The camera used during the loading screen.
        /// </summary>
        [SerializeField] private Camera loadingCamera;

        /// <summary>
        /// The array of scene groups to be loaded.
        /// </summary>
        [SerializeField] private SceneGroup[] SceneGroups;

        /// <summary>
        /// The active scene group.
        /// </summary>
        private SceneGroup _activeSceneGroup;
        
        private float targetProgress;
        private bool isLoading;

        /// <summary>
        /// The scene group manager instance.
        /// </summary>
        public static readonly SceneGroupManager SceneGroupManager = new SceneGroupManager();

        /// <summary>
        /// Initializes the scene loader.
        /// </summary>
        private void Awake()
        {
            // SceneGroupManager.OnSceneLoad += sceneName => Debug.Log($"Loaded scene: {sceneName}");
            // SceneGroupManager.OnSceneUnload += sceneName => Debug.Log($"Unloaded scene: {sceneName}");
            // SceneGroupManager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");

            // TODO: This race condition is not ideal. We should refactor this to be more robust.
            Bootstrapper.Instance.UpdateSceneLoader(this);
        }

        /// <summary>
        /// Starts the scene loading process.
        /// </summary>
        private async void Start()
        {
            await LoadSceneGroupIndex(0)!;
            await Task.Delay(2000);
        }

        /// <summary>
        /// Updates the loading bar fill amount.
        /// </summary>
        private void Update()
        {
            if (!isLoading || !loadingBar) return;

            float currentFillAmount = loadingBar.fillAmount;
            float progressDiff = Mathf.Abs(currentFillAmount - targetProgress);

            float dynamicFillSpeed = progressDiff * fillSpeed;

            loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        /// <summary>
        /// Loads the scene group at the specified index.
        /// </summary>
        /// <param name="index">The index of the scene group to load.</param>
        public async Task LoadSceneGroupIndex(int index)
        {
            if (loadingBar) loadingBar.fillAmount = 0f;
            targetProgress = 1f;

            if (index < 0 || index >= SceneGroups.Length)
            {
                Debug.LogError($"Invalid SceneGroup index: {index}");
                return;
            }

            await LoadSceneGroup(SceneGroups[index]);
        }

        /// <summary>
        /// Loads the specified scene group.
        /// </summary>
        /// <param name="group">The scene group to load.</param>
        public async Task LoadSceneGroup(SceneGroup group)
        {
            _activeSceneGroup = group;
            LoadingProgress progress = new LoadingProgress();
            progress.OnProgress += target => targetProgress = Mathf.Max(target, targetProgress);

            await EnableLoadingCanvas();
            await SceneGroupManager.LoadScenes(group, progress);
            await EnableLoadingCanvas(false);
        }

        /// <summary>
        /// Enables or disables the loading canvas.
        /// </summary>
        /// <param name="enable">If set to <c>true</c>, enables the loading canvas.</param>
        private async Task EnableLoadingCanvas(bool enable = true)
        {
            isLoading = enable;
            if (loadingCanvas) loadingCanvas.gameObject.SetActive(enable);
            // if (loadingCamera) loadingCamera.gameObject.SetActive(enable);

            if (enable)
            {
                UIManager.Instance.OnNodeTransitionStart?.Invoke();
            }
            else
            {
                UIManager.Instance.OnNodeTransitionEnd?.Invoke();
            }
            // TODO: Have a timeout so we don't get stuck here
            var timeout = 0;
            while (UIManager.Instance.IsTransitioning && timeout < 109)
            {
                // Wait for the transition to complete
                timeout++;
                await Task.Delay(100);
                if (timeout >= 100)
                {
                    Debug.LogError("Timeout reached while waiting for transition to complete.");
                }
            }
        }
        
        /// <summary>
        /// Reloads the currently active scene group.
        /// </summary>
        public async void ReloadActiveSceneGroup()
        {
            if (!_activeSceneGroup) return;
            await LoadSceneGroup(_activeSceneGroup);
        }

        /// <summary>
        /// Ends the current node.
        /// </summary>
        public async Task EndNode()
        {
            
            // Determine the next node
            // TODO: Currently just reloading the active scene group

            // Load the next node
            await LoadSceneGroup(_activeSceneGroup);
        }
        
        /// <summary>
        /// Gets the scene data for the specified subscene type.
        /// </summary>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public SceneData GetSubSceneData(SubSceneType sceneType)
        {
            return _activeSceneGroup.FindSceneBySubType(sceneType);
        }
    }

    /// <summary>
    /// Reports the loading progress.
    /// </summary>
    public class LoadingProgress : IProgress<float>
    {
        /// <summary>
        /// Event triggered when the progress is updated.
        /// </summary>
        public event Action<float> OnProgress;

        private const float ratio = 1f;

        /// <summary>
        /// Reports the progress value.
        /// </summary>
        /// <param name="value">The progress value.</param>
        public void Report(float value)
        {
            OnProgress?.Invoke(value / ratio);
        }
    }
}