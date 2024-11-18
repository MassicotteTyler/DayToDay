using System;
using SceneManagement;
using UnityEngine;

namespace Props
{
    /// <summary>
    /// Kill plane that reloads the scene when the player falls off the map.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class KillPlane : MonoBehaviour
    {
        /// <summary>
        /// Event invoked when the player falls off the map.
        /// </summary>
        public static event Action OnPlayerFellOffMap;
        
        /// <summary>
        /// The BoxCollider attached to this GameObject.
        /// </summary>
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }

        /// <summary>
        /// Reload the scene when the player falls off the map.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            // Check if player
            if (other.CompareTag("Player"))
            {
                Bootstrapper.Instance.SceneLoader?.ReloadActiveSceneGroup();
                OnPlayerFellOffMap?.Invoke();
            }
        }
    }
}