using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    /// <summary>
    ///    <para>First Person Controller</para>
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        
        /// <summary>
        ///   <para>Walking speed of the player</para>
        /// </summary>
        [Header("Movement")] [SerializeField] private float walkSpeed = 2.0f;
        
        /// <summary>
        ///  <para>Standaing height of the player</para>
        /// </summary>
        [SerializeField] private float standingHeight = 2.0f;

        /// <summary>
        /// <para>Sensitivity of the mouse</para>
        /// </summary>
        [Header("Mouse")] [SerializeField] private float mouseSensitivity = 2.0f;
        
        /// <summary>
        /// The maximum angle the player can look up or down
        /// </summary>
        [SerializeField] private float maxLookAngle = 90.0f;
        
        /// <summary>
        /// Invert the Y axis
        /// </summary>
        [SerializeField] private bool invertY = false;

        /// <summary>
        /// The distance to check if the player is grounded
        /// </summary>
        [Header("Ground Check")] [SerializeField]
        private LayerMask groundMask;

        /// <summary>
        /// The distance to check if the player is grounded
        /// </summary>
        [SerializeField] private float groundCheckDistance = 0.4f;

        /// <summary>
        /// The character controller component
        /// </summary>
        private CharacterController _characterController;
        
        /// <summary>
        /// The player camera
        /// </summary>
        private Camera _playerCamera;

        // Movement
        /// <summary>
        /// The movement direction of the player
        /// </summary>
        private Vector3 _moveDirection;
        
        /// <summary>
        /// The velocity of the player
        /// </summary>
        private Vector3 _velocity;
        
        /// <summary>
        /// The current speed of the player
        /// </summary>
        private float _currentSpeed;

        // Mouse Look
        /// <summary>
        /// The rotation of the player on the X axis
        /// </summary>
        private float _rotationX = 0f;
        
        /// <summary>
        /// The rotation of the player on the Y axis
        /// </summary>
        private float _rotationY = 0f;

        // Start is called before the first frame update
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _playerCamera = GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        private void Update()
        {
            HandleInput();
            HandleMovement();
            HandleMouseLook();
            CheckGround();
        }

        private void OnDisable()
        {
            
        }


        /// <summary>
        ///  <para>Handles the player input</para>
        /// </summary>
        private void HandleInput()
        {
            // Movement
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            _moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        }

        /// <summary>
        /// <para>Check if the player is grounded</para>
        /// </summary>
        private void CheckGround()
        {
        }

        /// <summary>
        /// <para>Handles the player movement</para>
        /// </summary>
        private void HandleMovement()
        {
            _currentSpeed = walkSpeed;
            _velocity = transform.TransformDirection(_moveDirection) * _currentSpeed;
            _velocity.y -= 9.81f;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        /// <summary>
        /// <para>Handles the player mouse look</para>
        /// </summary>
        private void HandleMouseLook()
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            _rotationX -= mouseY;
            _rotationX = Mathf.Clamp(_rotationX, -maxLookAngle, maxLookAngle);

            _rotationY += mouseX;

            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation = Quaternion.Euler(0, _rotationY, 0);
        }
    }
}