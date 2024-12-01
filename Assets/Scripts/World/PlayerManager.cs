using System;
using System.Collections.Generic;
using Controller;
using Events;
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
        
        /// <summary>
        /// The <see cref="EndGameEvent"/>'s discovered by the player.
        /// </summary>
        public Dictionary<string, EndGameEvent> EndGameEvents { get; private set; } = new Dictionary<string, EndGameEvent>();
        
        /// <summary>
        /// The current <see cref="EndGameEvent"/>.
        /// </summary>
        public EndGameEvent CurrentEndGameEvent { get; private set; }

        /// <summary>
        /// If the game has ended.
        /// </summary>
        public bool IsGameEnded = false;
        
        private void Awake()
        {
           FPSController.OnPlayerSpawned += HandlePlayerSpawned;
           EndGameEvent.OnGameEnd += HandleGameEnd;
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
                Destroy(PlayerController.gameObject);
            }

            PlayerController = playerController;
            PlayerController?.Init();
        }
        
        /// <summary>
        /// Handles the game end event.
        /// </summary>
        /// <param name="endGameEvent">The <see cref="EndGameEvent"/> that was triggered</param>
        private void HandleGameEnd(EndGameEvent endGameEvent)
        {
            CurrentEndGameEvent = endGameEvent;
            if (EndGameEvents.TryAdd(endGameEvent.GameEndingName, endGameEvent))
            {
                // New ending discovered
                // TODO: Do we want an event for this?
            }
            
            IsGameEnded = true;
            Bootstrapper.Instance.SceneLoader.EndGame();
        }
    }
}