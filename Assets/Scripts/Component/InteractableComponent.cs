using System;
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
       void Observer(GameObject observer);
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
        
        public void Interact(GameObject interactor)
        {
            onInteract?.Invoke(interactor);
        }

        public void Observer(GameObject observer)
        {
            onObserve?.Invoke(observer);    
        }
    }
}