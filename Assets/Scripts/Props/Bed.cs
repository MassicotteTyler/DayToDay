﻿using System;
using System.Collections;
using Component;
using Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace Props
{
    /// <summary>
    /// Bed prop that can be interacted with.
    /// </summary>
    [RequireComponent(typeof(InteractableComponent))]
    public class Bed : MonoBehaviour
    {
        /// <summary>
        /// The InteractableComponent attached to this GameObject.
        /// </summary>
        private InteractableComponent _interactableComponent;

        /// <summary>
        /// The camera used for the sleeping view.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The transform where the occupier will be positioned when sleeping.
        /// </summary>
        [SerializeField] private Transform sleepTransform;
        
        /// <summary>
        /// The duration of the camera move coroutine.
        /// </summary>
        [SerializeField] private float cameraMoveDuration = 2.5f;

        /// <summary>
        /// The position where the player will be positioned when sleeping.
        /// </summary>
        private Vector3 _sleepPosition;

        /// <summary>
        /// The rotation where the occupier will be positioned when sleeping.
        /// </summary>
        private Quaternion _sleepRotation;

        /// <summary>
        /// The GameObject currently occupying the bed.
        /// </summary>
        private GameObject _occupiedBy;

        /// <summary>
        /// Action to be invoked when the camera move is complete.
        /// </summary>
        private Action _onCameraMoveComplete;

        /// <summary>
        /// Initializes the Bed component.
        /// </summary>
        private void Start()
        {
            _interactableComponent = GetComponent<InteractableComponent>();
            _interactableComponent.onInteract?.AddListener(Interact);

            _camera = GetComponentInChildren<Camera>();
            _sleepPosition = sleepTransform.position;
            _sleepRotation = sleepTransform.rotation;
            _camera.enabled = false;
        }

        /// <summary>
        /// Handles the interaction with the bed.
        /// </summary>
        /// <param name="interactor">The GameObject that interacts with the bed.</param>
        private void Interact(GameObject interactor)
        {
            if (_occupiedBy) return;

            var player = interactor.GetComponent<FPSController>();
            var playerCamera = interactor.GetComponentInChildren<Camera>();
            if (!player || !playerCamera) return;

            playerCamera.enabled = false;
            _camera.enabled = true;
            _camera.transform.position = playerCamera.transform.position;
            _camera.transform.rotation = playerCamera.transform.rotation;
            player.gameObject.SetActive(false);
            _occupiedBy = interactor;
            StartCoroutine(MoveCameraToTransform(_sleepPosition, _sleepRotation));

            // This can be removed once we have sleep events and node transitions
            _onCameraMoveComplete += () =>
            {
                Invoke(nameof(WakeUp), 1.5f);
                _onCameraMoveComplete = null;
            };
        }

        /// <summary>
        /// Coroutine to move the camera to the sleep transform.
        /// </summary>
        /// <param name="targetPosition">The target position for the camera.</param>
        /// <param name="targetRotation">The target rotation for the camera.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator MoveCameraToTransform(Vector3 targetPosition, Quaternion targetRotation)
        {
            var t = 0f;
            var startPosition = _camera.transform.position;
            var startRotation = _camera.transform.rotation;

            while (t < cameraMoveDuration)
            {
                t += Time.fixedDeltaTime;
                _camera.transform.position = Vector3.Lerp(startPosition, targetPosition, t / cameraMoveDuration);
                _camera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t / cameraMoveDuration);
                yield return null;
            }
            _onCameraMoveComplete?.Invoke();
        }

        /// <summary>
        /// Wakes up the player from the bed.
        /// </summary>
        private void WakeUp()
        {
            var player = _occupiedBy?.GetComponent<FPSController>();
            var playerCamera = _occupiedBy?.GetComponentInChildren<Camera>();
            if (!player || !playerCamera) return;

            StartCoroutine(MoveCameraToTransform(playerCamera.transform.position, playerCamera.transform.rotation));

            _onCameraMoveComplete += () =>
            {
                playerCamera.enabled = true;
                _camera.enabled = false;
                player.gameObject.SetActive(true);
                _occupiedBy = null;
                _onCameraMoveComplete = null;
            };
        }
    }
}