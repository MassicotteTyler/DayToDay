using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Props.Trigger
{
    /// <summary>
    /// Trigger that can be attached to a GameObject to trigger events when the object enters or exits the trigger.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class TagTrigger : MonoBehaviour
    {
        /// <summary>
        /// The collider for the trigger.
        /// </summary>
        private BoxCollider _collider;
        
        /// <summary>
        /// Event that is triggered when the gameObject enters the trigger.
        /// </summary>
        public UnityEvent<GameObject> OnObjectEnter;
        
        /// <summary>
        /// Event that is triggered when the object exits the trigger.
        /// </summary>
        public UnityEvent<GameObject> OnObjectExit;
        
        /// <summary>
        /// If set to <c>true</c>, the trigger will only trigger once and be removed.
        /// </summary>
        [SerializeField] private bool _isOneTimeTrigger = false;
        
        /// <summary>
        /// If set to <c>true</c>, the object that invokes the trigger will be destroyed.
        /// </summary>
        [SerializeField] private bool _destroyInvokingObject = false;
        
        /// <summary>
        /// The <see cref="GameEvent"/> to invoke when the object enters the trigger.
        /// </summary>
        [SerializeField] private List<GameEvent> _eventsToTrigger;
        
        /// <summary>
        /// The tags that will trigger the events.
        /// </summary>
        [SerializeField] private List<string> _tagsToTrigger;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }

        /// <summary>
        /// Unity's OnTriggerEnter method. Triggers events when the object enters the trigger.
        /// </summary>
        /// <param name="other">The Collider object that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!_tagsToTrigger.Contains(other.tag)) return;
            OnObjectEnter?.Invoke(other.gameObject);
            TriggerEvents();
                
            if (_isOneTimeTrigger)
            {
                Destroy(gameObject);
            }
            
            if (_destroyInvokingObject)
            {
                Destroy(other.gameObject);
            }
        }
        
        /// <summary>
        /// Unity's OnTriggerExit method. Triggers events when the object exits the trigger.
        /// </summary>
        /// <param name="other">The Collider object that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (_tagsToTrigger.Contains(other.tag))
            {
                OnObjectExit?.Invoke(other.gameObject);
            }
        }
        
        /// <summary>
        /// Invokes the events that are set to trigger when the object enters the trigger.
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