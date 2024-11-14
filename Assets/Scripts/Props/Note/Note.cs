using Component;
using UI;
using UnityEngine;

namespace Props.Note
{
    /// <summary>
    /// Note prop that can be interacted with.
    /// </summary>
    [RequireComponent(typeof(InteractableComponent))]
    public class Note : MonoBehaviour
    {
        /// <summary>
        /// The InteractableComponent attached to this GameObject.
        /// </summary>
        private InteractableComponent _interactableComponent;
        /// <summary>
        /// The text to display when the note is interacted with.
        /// </summary>
        [SerializeField] [TextArea]  private string noteText;

        private void Start()
        {
            _interactableComponent = GetComponent<InteractableComponent>();
            _interactableComponent.onInteract?.AddListener(Interact);
        }

        private void Interact(GameObject interactor)
        {
            UIManager.Instance.OnNoteTextChange?.Invoke(noteText);
        }

        private void OnDestroy()
        {
            _interactableComponent?.onInteract?.RemoveListener(Interact);
        }
    }
}