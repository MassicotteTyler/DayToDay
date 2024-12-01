using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Events;

namespace Props.Trigger
{
    /// <summary>
    /// Trigger that can be attached to a GameObject to trigger events when the player enters or exits the trigger.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerTrigger : MonoBehaviour
    {
        /// <summary>
        /// The collider for the trigger.
        /// </summary>
        private BoxCollider _collider;
        
        /// <summary>
        /// Event that is triggered when the player enters the trigger.
        /// </summary>
        public UnityEvent<GameObject> OnPlayerEnter;
        
        /// <summary>
        /// Event that is triggered when the player exits the trigger.
        /// </summary>
        public UnityEvent<GameObject> OnPlayerExit;
        
        /// <summary>
        /// If set to <c>true</c>, the trigger will only trigger once and be removed.
        /// </summary>
        [SerializeField] private bool _isOneTimeTrigger = false;
        
        /// <summary>
        /// The <see cref="GameEvent"/> to invoke when the player enters the trigger.
        /// </summary>
        [SerializeField] private List<GameEvent> _eventsToTrigger;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }

        /// <summary>
        /// Unity's OnTriggerEnter method. Triggers events when the player enters the trigger.
        /// </summary>
        /// <param name="other">The Collider object that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnPlayerEnter?.Invoke(other.gameObject);
            TriggerEvents();
                
            if (_isOneTimeTrigger)
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Unity's OnTriggerExit method. Triggers events when the player exits the trigger.
        /// </summary>
        /// <param name="other">The Collider object that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerExit?.Invoke(other.gameObject);
            }
        }
        
        /// <summary>
        /// Invokes the events that are set to trigger when the player enters the trigger.
        /// </summary>
        private void TriggerEvents()
        {
            foreach (var gameEvent in _eventsToTrigger)
            {
                gameEvent?.Invoke();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider>();
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + _collider.center, _collider.size);
        }
#endif
    }
}