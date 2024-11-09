using System;
using Component;
using UnityEngine;

namespace Props
{
    /// <summary>
    /// Box prop that can be interacted with.
    /// </summary>
    [RequireComponent(typeof(InteractableComponent))]
    public class Box : MonoBehaviour
    {
        private InteractableComponent _interactableComponent;

        private void Start()
        {
            _interactableComponent = GetComponent<InteractableComponent>();
            _interactableComponent.onInteract?.AddListener(Interact);
        }

        private void Interact(GameObject interactor)
        {
            Debug.Log("Interacting with box");
        }


        private void OnDestroy()
        {
            _interactableComponent?.onInteract?.RemoveListener(Interact);
        }
    }
}