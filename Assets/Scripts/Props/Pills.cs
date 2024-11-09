using System;
using Component;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Props
{
    [RequireComponent(typeof(InteractableComponent))]
    public class Pills : MonoBehaviour
    {
       private InteractableComponent _interactableComponent;

       private void Start()
       {
              _interactableComponent = GetComponent<InteractableComponent>();
              _interactableComponent.onInteract?.AddListener(Interact);
       }
       
       public void Interact(GameObject interactor)
       {
              Debug.Log("Interacting with pills");
       }
    }
}