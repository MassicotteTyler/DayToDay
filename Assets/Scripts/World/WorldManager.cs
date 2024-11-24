using System;
using Events;
using UI;
using UnityEngine;
using Utility;

namespace World
{
    /// <summary>
    /// The state of the world.
    /// </summary>
    public class WorldState
    {
        /// <summary>
        /// The time of day.
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Count of days elapsed.
        /// </summary>
        public int Day { get; set; } = 1;
        
        /// <summary>
        /// The current node.
        /// </summary>
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
        
        public struct GameState
        {
            public WorldState World;
            public PlayerState Player;
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
        /// Gets the current game state.
        /// </summary>
        /// <returns>The current <see cref="GameState"/></returns>
        public GameState GetGameState()
        {
            return new GameState
            {
                World = _worldState,
                Player = _playerState
            };
        }
        
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
            EndNodeEvent.OnEndNode += HandleNodeEnd;
            
            UIManager.Instance.OnNodeTransitionEnd += PayPlayer;
        }

        
        private void OnDestroy()
        {
            ConsumedPillsEvent.onPillsConsumed -= OnPillsConsumed;
            EndNodeEvent.OnEndNode -= HandleNodeEnd;
        }
        
        /// <summary>
        /// Handle end of a node. Should be used to process state. 
        /// </summary>
        private void HandleNodeEnd()
        {
            _worldState.Day++;
            PayPlayer();
        }

        /// <summary>
        /// Pays the player for work.
        /// </summary>
        public void PayPlayer()
        {
            // TODO: this should be an event that listens for the EndNode event
            
            if (_playerState.ItemsShelved > 2)
            {
                UpdatePlayerMoney(_playerState.Money + 100f);
            }
            _playerState.ItemsShelved = 0;
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