using System;
using UnityEngine;

namespace Component
{
    /// <summary>
    /// Component that allows the owning object to interact with interactable objects.
    /// </summary>
    public class InteractorComponent : MonoBehaviour
    {
        // TODO: We need a way to prevent the player interacting again while already interacting
        /// <summary>
        /// Flag to check if the player is currently interacting with an object.
        /// </summary>
        private bool _isInteracting = false;
        
        /// <summary>
        /// Key to press to interact with objects.
        /// </summary>
        [SerializeField] private KeyCode interactKey = KeyCode.F;
        
        /// <summary>
        /// Distance to check for interactable objects.
        /// </summary>
        [Range(0, 10)]
        [SerializeField] private float interactDistance = 2f;
        
        private void Start()
        {
        }
        
        private void Update()
        {
            CheckForInteractable();
            if (!_isInteracting  && Input.GetKeyDown(interactKey))
            {
                Interact();
            }   
        }
        
        /// <summary>
        /// Check for interactable objects in front of the player.
        /// </summary>
        private void CheckForInteractable()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, interactDistance))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.Observer(gameObject);
            }
        }
        
        /// <summary>
        /// Casts a ray in front of the player and interacts with the first interactable object found.
        /// </summary>
        private void Interact()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, interactDistance))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                // pass in owner of the interactor component
                interactable?.Interact(gameObject);
            }
        }
    }
}