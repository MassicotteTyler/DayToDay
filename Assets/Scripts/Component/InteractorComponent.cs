using System;
using UnityEngine;
using UnityEngine.Scripting;

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
        /// Object currently being observed by the interactor.
        /// </summary>
        private IInteractable _observedObject;
        
        /// <summary>
        /// Key to press to interact with objects.
        /// </summary>
        [SerializeField] private KeyCode interactKey = KeyCode.F;
        
        /// <summary>
        /// Distance to check for interactable objects.
        /// </summary>
        [Range(0, 10)]
        [SerializeField] private float interactDistance = 2f;
        
        /// <summary>
        /// Transform to use as the view point for the interactor.
        /// </summary>
        [SerializeField] private Transform _viewPoint;
        
        private void Start()
        {
            if (_viewPoint == null)
            {
                _viewPoint = transform;
            }
        }
        
        private void Update()
        {
            CheckForInteractable();
            if (!_isInteracting  && Input.GetKeyDown(interactKey))
            {
                Interact();
            }   
        }

        private void OnDisable()
        {
           _observedObject?.StopObserving(gameObject); 
           _observedObject = null;
        }

        /// <summary>
        /// Casts a ray in front of the interactor to check for interactable objects.
        /// </summary>
        /// <param name="hit">RaycastHit result</param>
        /// <param name="interactable">Interactable component if found</param>
        /// <returns></returns>
        private bool LookAtInteractable(out RaycastHit hit, out IInteractable interactable)
        {
            var result = Physics.Raycast(_viewPoint.position, _viewPoint.forward, out hit, interactDistance);
            interactable = hit.collider?.GetComponent<IInteractable>();
            return result &&  interactable != null;
        }
        
        /// <summary>
        /// Check for interactable objects in front of the player.
        /// </summary>
        private void CheckForInteractable()
        {
            if (!LookAtInteractable(out var hit, out var interactable))
            {
                _observedObject?.StopObserving(gameObject);
                _observedObject = null;
                return;
            }
            
            if (_observedObject == interactable) return;
            _observedObject?.StopObserving(gameObject);
            _observedObject = interactable;
            _observedObject?.Observe(gameObject);
        }
        
        /// <summary>
        /// Casts a ray in front of the player and interacts with the first interactable object found.
        /// </summary>
        private void Interact()
        {
            if (!LookAtInteractable(out var hit, out var interactable)) return;
            {
                // pass in owner of the interactor component
                interactable?.Interact(gameObject);
            }
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_viewPoint.position, _viewPoint.forward * interactDistance);
        }
        #endif
    }
}