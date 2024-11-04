using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Manager for UI elements in the game.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the UIManager.
        /// </summary>
        private static UIManager _instance;

        /// <summary>
        /// Action to trigger the interaction label to change.
        /// </summary>
        public Action<string> OnInteractionLabelChanged;

        /// <summary>
        /// Action to trigger the interaction label to hide.
        /// </summary>
        public Action OnHideInteractionLabel;

        /// <summary>
        /// Singleton accessor of the instance.
        /// </summary>
        public static UIManager Instance => _instance ??= GameObject.AddComponent<UIManager>();

        /// <summary>
        /// Bootstrap the UIManager. Runs before the first scene is loaded.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Bootstrap()
        {
            Instance.Initialize();
        }

        /// <summary>
        /// Initialize the UIManager.
        /// </summary>
        private void Initialize()
        {
            Debug.Log("UIManager initialized");
        }

        /// <summary>
        /// GameObject for the UIManager. Will ensure a game object exists for the UIManager.
        /// </summary>
        private static GameObject GameObject
        {
            get
            {
                if (_instance != null)
                {
                    return _instance.gameObject;
                }

                var go = new GameObject(nameof(UIManager));
                DontDestroyOnLoad(go);
                return go;
            }
        }
    }
}