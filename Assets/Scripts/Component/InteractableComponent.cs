using System;
using System.Collections.Generic;
using Events;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Component
{
    /// <summary>
    /// Interface for objects that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Method to be called when the object is interacted with.
        /// </summary>
        /// <param name="interactor">The GameObject that triggered the interaction</param>
        void Interact(GameObject interactor);

        /// <summary>
        /// Method to be called when the object is observed.
        /// </summary>
        /// <param name="observer">The GameObject that triggered the observation</param>
        void Observe(GameObject observer);

        /// <summary>
        /// Method to be called when the object is no longer observed.
        /// </summary>
        /// <param name="observer">GameObject that observed this object</param>
        void StopObserving(GameObject observer);
    }

    /// <summary>
    /// Component that allows the owning object to interact with interactable objects.
    /// </summary>
    public class InteractableComponent : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// Action to be invoked when the owning object is interacted with.
        /// </summary>
        public UnityEvent<GameObject> onInteract;

        /// <summary>
        /// Action to be invoked when the owning object is observed.
        /// </summary>
        public UnityEvent<GameObject> onObserve;

        /// <summary>
        /// Action to be invoked when the owning object is no longer observed.
        /// </summary>
        public UnityEvent<GameObject> onStopObserving;

        /// <summary>
        /// Label to display when the object is observed.
        /// </summary>
        [SerializeField] private string interactionLabel = "Interact";

        /// <summary>
        /// Events to trigger when the object is interacted with.
        /// </summary>
        [SerializeField] private List<GameEvent> InteractionEvents;

        public void Interact(GameObject interactor)
        {
            onInteract?.Invoke(interactor);
            
            foreach (var interactionEvent in InteractionEvents)
            {
                interactionEvent.Invoke(gameObject);
            }
        }

        public void Observe(GameObject observer)
        {
            onObserve?.Invoke(observer);
            UIManager.Instance.OnInteractionLabelChanged?.Invoke(interactionLabel);
        }

        public void StopObserving(GameObject observer)
        {
            onStopObserving?.Invoke(observer);
            UIManager.Instance.OnHideInteractionLabel?.Invoke();
        }
        
        public void SetInteractionLabel(string label)
        {
            interactionLabel = label;
            UIManager.Instance.OnInteractionLabelChanged?.Invoke(interactionLabel);
        }
    }
}