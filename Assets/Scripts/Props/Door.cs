using System;
using System.Collections;
using System.Collections.Generic;
using Component;
using UnityEngine;
using UnityEngine.Events;

namespace Props
{
    /// <summary>
    /// A component that represents a door which can be interacted with.
    /// </summary>
    [RequireComponent(typeof(InteractableComponent))]
    [RequireComponent(typeof(BoxCollider))]
    public class Door : MonoBehaviour
    {
        /// <summary>
        /// Indicates whether the door is open.
        /// </summary>
        [SerializeField] private bool _isOpen = false;

        /// <summary>
        /// The angle at which the door is open.
        /// </summary>
        [SerializeField] private float openAngle = 90f;

        /// <summary>
        /// The angle at which the door is closed.
        /// </summary>
        [SerializeField] private float closeAngle = 0f;

        /// <summary>
        /// The speed at which the door opens and closes.
        /// </summary>
        [SerializeField] private float speed = .3f;

        /// <summary>
        /// Indicates whether the door is locked.
        /// </summary>
        [SerializeField] private bool isLocked = false;

        /// <summary>
        /// The transform around which the door pivots.
        /// </summary>
        [SerializeField] private Transform pivotTransform;

        /// <summary>
        /// Event triggered when the state of the door changes.
        /// </summary>
        public UnityEvent onStateChange;

        /// <summary>
        /// Event triggered when the door is opened.
        /// </summary>
        public UnityEvent onOpen;

        /// <summary>
        /// Event triggered when the door is closed.
        /// </summary>
        public UnityEvent onClose;

        /// <summary>
        /// The interactable component of the door.
        /// </summary>
        private InteractableComponent _interactableComponent;
        
        /// <summary>
        /// The box collider of the door.
        /// </summary>
        private BoxCollider _boxCollider;

        /// <summary>
        /// Gets or sets a value indicating whether the door is open.
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            private set
            {
                _isOpen = value;
                _interactableComponent.SetInteractionLabel(InteractionStateText);
                onStateChange?.Invoke();
            }
        }

        /// <summary>
        /// Gets the interaction state text based on the door's state.
        /// </summary>
        public string InteractionStateText
        {
            get
            {
                if (isLocked) return "Locked";
                return !_isOpen ? "Open" : "Close";
            }
        }

        /// <summary>
        /// Initializes the door component.
        /// </summary>
        private void Start()
        {
            _interactableComponent = GetComponent<InteractableComponent>();
            _interactableComponent.onInteract?.AddListener((interactor) => Toggle());
            _interactableComponent.SetInteractionLabel(InteractionStateText);

            _boxCollider = GetComponent<BoxCollider>();
        }

        /// <summary>
        /// Cleans up the door component.
        /// </summary>
        private void OnDestroy()
        {
            _interactableComponent.onInteract?.RemoveAllListeners();
        }

        private void Update()
        {
        }

        /// <summary>
        /// Coroutine for transitioning the door between open and closed states.
        /// </summary>
        /// <param name="startAngle">The starting angle of the door.</param>
        /// <param name="endAngle">The ending angle of the door.</param>
        /// <param name="duration">The duration of the transition.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator TransitionDoor(float startAngle, float endAngle, float duration)
        {
            float time = 0;
            _boxCollider.enabled = false;
            while (time < duration)
            {
                var angle = Mathf.Lerp(startAngle, endAngle, time / duration);
                transform.RotateAround(pivotTransform.position, Vector3.up, angle - transform.localRotation.eulerAngles.y);
                time += Time.deltaTime;
                yield return null;
            }
            _boxCollider.enabled = true;
            transform.RotateAround(pivotTransform.position, Vector3.up, endAngle - transform.localRotation.eulerAngles.y);
        }

        /// <summary>
        /// Toggles the door between open and closed states.
        /// </summary>
        public void Toggle()
        {
            if (_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        /// <summary>
        /// Sets the locked state of the door.
        /// </summary>
        /// <param name="locked">If set to <c>true</c>, the door is locked.</param>
        public void SetLocked(bool locked)
        {
            isLocked = locked;
            _interactableComponent.SetInteractionLabel(InteractionStateText);
        }

        /// <summary>
        /// Opens the door.
        /// </summary>
        /// <param name="forceOpen">If set to <c>true</c>, the door will open even if it is locked.</param>
        public void Open(bool forceOpen = false)
        {
            if (isLocked && !forceOpen) return;
            if (_isOpen) return;

            IsOpen = true;
            onOpen?.Invoke();

            StartCoroutine(TransitionDoor(closeAngle, openAngle, speed));
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        public void Close()
        {
            if (!_isOpen) return;

            IsOpen = false;
            onClose?.Invoke();

            StartCoroutine(TransitionDoor(openAngle, closeAngle, speed));
        }
    }
}