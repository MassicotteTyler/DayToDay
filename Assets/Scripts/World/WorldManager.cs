using System;
using Events;
using UnityEngine;
using Utility;

namespace World
{
    /// <summary>
    /// State manager for the things outside of the nodes.
    /// </summary>
    public class WorldManager : Singleton<WorldManager>
    {
        /// <summary>
        ///   Action to trigger when the player's money changes.
        /// </summary>
        public Action<float> OnPlayerMoneyChanged;
        
        /// <summary>
        /// The state of the world.
        /// </summary>
        public class WorldState
        {
            public float Time { get; set; }
            public string Weather { get; set; }
            public string Node { get; set; }
        }

        /// <summary>
        /// The state of the player.
        /// </summary>
        public class PlayerState
        {
            /// <summary>
            /// The player's money.
            /// </summary>
            public float Money { get; set; } = 0f;
            
            /// <summary>
            /// The number of items shelved by the player.
            /// </summary>
            public int ItemsShelved { get; set; } = 0;
            
            /// <summary>
            /// If the player has consumed pills.
            /// </summary>
            public bool HasConsumedPills { get; set; } = false;
            
            /// <summary>
            /// The number of pills consumed by the player.
            /// </summary>
            public int  ConsumedPills { get; set; } = 0;
        }
        
        /// <summary>
        /// The current state of the world.
        /// </summary>
        private WorldState _worldState = new WorldState();
        
        /// <summary>
        /// The current state of the player.
        /// </summary>
        private PlayerState _playerState = new PlayerState();
        
        /// <summary>
        /// Updates the player's money.
        /// </summary>
        /// <param name="amount">New amount</param>
        public void UpdatePlayerMoney(float amount)
        {
            _playerState.Money = amount;
            OnPlayerMoneyChanged?.Invoke(amount);
        }
        
        /// <summary>
        /// Gets the player's money.
        /// </summary>
        /// <returns>Player's Money</returns>
        public float GetPlayerMoney()
        {
            return _playerState.Money;
        }

        /// <summary>
        /// Gets the number of items shelved by the player.
        /// </summary>
        public void PlayerShelvedItem()
        {
            ++_playerState.ItemsShelved;
        }

        private void Awake()
        {
            ConsumedPillsEvent.onPillsConsumed += OnPillsConsumed;
        }
        
        
        private void OnDestroy()
        {
            ConsumedPillsEvent.onPillsConsumed -= OnPillsConsumed;
        }
        
        /// <summary>
        /// Event handler for when pills are consumed.
        /// </summary>
        private void OnPillsConsumed()
        {
            _playerState.HasConsumedPills = true;
            _playerState.ConsumedPills++;
        }
    }
}