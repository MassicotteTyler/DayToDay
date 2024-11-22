using System;
using Controller;
using SceneManagement;
using UnityEngine;
using Utility;

namespace World
{
    /// <summary>
    /// Manager for the player.
    /// </summary>
    public class PlayerManager : Singleton<PlayerManager>
    {
        /// <summary>
        /// The current <see cref="FPSController"/>.
        /// </summary>
        public FPSController PlayerController { get; private set; }
        
        private void Awake()
        {
           FPSController.OnPlayerSpawned += HandlePlayerSpawned;
        }

        private void OnDestroy()
        {
            FPSController.OnPlayerSpawned -= HandlePlayerSpawned;
        }

        /// <summary>
        /// Handles the player spawned event.
        /// </summary>
        /// <param name="playerController">The spawned <see cref="FPSController"/></param>
        private void HandlePlayerSpawned(FPSController playerController)
        {
            if (PlayerController && PlayerController != playerController)
            {
                // We only want one player controller at a time.
                DestroyImmediate(PlayerController.gameObject);
            }
            PlayerController = playerController;
        }
    }
}