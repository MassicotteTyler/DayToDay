using System;
using System.Collections.Generic;
using Bodega;
using Events;
using SceneManagement;
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
        public float Money { get; set; } = 0;

        ///<summary>
        /// If the player worked today.
        /// </summary>
        public bool PlayerWorkedToday { get; set; } = false;

        ///<summary>
        /// If the player worked the previous day.
        /// </summary>
        public bool WorkedPreviousDay { get; set; } = false;

        /// <summary>
        /// The number of items shelved by the player.
        /// </summary>
        public int ItemsShelved { get; set; } = 0;
        
        /// <summary>
        /// If the player has consumed pills.
        /// </summary>
        public bool HasConsumedPills { get; set; } = false;
        
        /// <summary>
        /// If the player consumed pills the previous day.
        /// </summary>
        public bool HasConsumedPillsPrevDay { get; set; } = false;
        
        /// <summary>
        /// The number of pills consumed by the player.
        /// </summary>
        public int  ConsumedPills { get; set; } = 0;

        /// <summary>
        /// If the player has seen the green man.
        /// </summary
        public bool HasSeenGreenMan { get; set; } = false;

        /// <summary>
        /// If the player has toggled the front door.
        /// </summary>
        public bool ToggledFrontDoor { get; set; } = false;
        
        /// <summary>
        /// If the player has completed their job.
        /// </summary>
        public bool JobCompleted { get; set; } = false;
        
        /// <summary>
        /// The visited nodes.
        /// </summary>
        public Dictionary<string, SceneGroup> visitedNodes = new Dictionary<string, SceneGroup>();
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

        /// <summary>
        /// Resets WorldManager state
        /// </summary>
        public void ResetGameState()
        {
            _worldState = new WorldState();
            _playerState = new PlayerState();

            OnPlayerMoneyChanged?.Invoke(_playerState.Money);
            _playerState.WorkedPreviousDay = false;
        }

        private void Awake()
        {
            ConsumedPillsEvent.onPillsConsumed += OnPillsConsumed;
            SeenGreenManEvent.seenGreenMan += OnSeenGreenMan;
            FrontDoorToggledEvent.frontDoorToggled += OnToggledFrontDoor;
            EndNodeEvent.OnEndNode += HandleNodeEnd;
            BodegaManager.OnJobCompleted += PayPlayer;

            // Start the day when the node transition ends.
            UIManager.Instance.OnNodeTransitionEnd += StartDay;
            
            SceneGroupManager.OnSceneGroupLoaded += HandleNodeLoaded;
        }
        
        private void OnDestroy()
        {
            ConsumedPillsEvent.onPillsConsumed -= OnPillsConsumed;
            SeenGreenManEvent.seenGreenMan -= OnSeenGreenMan;
            EndNodeEvent.OnEndNode -= HandleNodeEnd;
            BodegaManager.OnJobCompleted -= PayPlayer;
            UIManager.Instance.OnNodeTransitionEnd -= StartDay;
            
            SceneGroupManager.OnSceneGroupLoaded -= HandleNodeLoaded;
        }
        
        /// <summary>
        /// Handle end of a node. Should be used to process state. 
        /// </summary>
        private void HandleNodeEnd()
        {
            _worldState.Day++;
            _playerState.HasConsumedPillsPrevDay = _playerState.HasConsumedPills;
            _playerState.WorkedPreviousDay = _playerState.PlayerWorkedToday;
        }

        private void HandleNodeLoaded(SceneGroup sceneGroup)
        {
            _worldState.Node = sceneGroup.name;
            if (_playerState.visitedNodes.TryAdd(sceneGroup.name, sceneGroup))
            {
                Debug.Log($"Visited {sceneGroup.name}");
            }
        }
        private void StartDay()
        {
            _playerState.JobCompleted = false;
            _playerState.HasConsumedPills = false;
            _playerState.ToggledFrontDoor = false;
            _playerState.PlayerWorkedToday = false;
        }

        /// <summary>
        /// Pays the player for work.
        /// </summary>
        public void PayPlayer()
        {
            _playerState.JobCompleted = true;
            UpdatePlayerMoney(_playerState.Money + 100f);
            _playerState.PlayerWorkedToday = true;
        }

        /// <summary>
        /// Event handler for when pills are consumed.
        /// </summary>
        private void OnPillsConsumed()
        {
            _playerState.HasConsumedPills = true;
            _playerState.ConsumedPills++;
        }

        private void OnSeenGreenMan()
        {
            _playerState.HasSeenGreenMan = true;
        }

        private void OnToggledFrontDoor()
        {
            _playerState.ToggledFrontDoor = !_playerState.ToggledFrontDoor;
        }
    }
}