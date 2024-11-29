using System;
using System.Collections;
using UI;
using UnityEngine;
using World;

namespace Props.Player
{
    /// <summary>
    /// Moves the player gameobjet to the spawn point.
    /// </summary>
    public class PlayerSpawner : MonoBehaviour
    {
        /// <summary>
        /// Should this spawn position be a camera position
        /// Camera position means no player Init or movement
        /// </summary>
        public bool CameraMode = false;

        private void Awake()
        {
           StartCoroutine(WaitForPlayerController());
           UIManager.Instance.OnNodeTransitionEnd += MovePlayerToSpawn;
        }

        private void Update()
        {
           #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Moving player to spawn point.");
                MovePlayerToSpawn();
            }
            #endif
        }

        private void OnDestroy()
        {
            UIManager.Instance.OnNodeTransitionEnd -= MovePlayerToSpawn;
        }

        /// <summary>
        /// Waits for the player controller to be initialized.
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitForPlayerController()
        {
            while (!PlayerManager.Instance.PlayerController)
            {
                yield return null;
            }
            PlayerManager.Instance.PlayerController.MovementEnabled = false;
            MovePlayerToSpawn();
        }

        /// <summary>
        /// Moves the player to the spawn point.
        /// </summary>
        private void MovePlayerToSpawn()
        {
            var playerController = PlayerManager.Instance.PlayerController;
            
            if (!playerController)
            {
                Debug.LogError("PlayerController not found.");
                return;
            }
           
            // Move the player to the spawn point.
            playerController.gameObject.SetActive(false);
            playerController.transform.SetPositionAndRotation(transform.position, transform.rotation);
            playerController.MovementEnabled = !CameraMode;
            playerController.gameObject.SetActive(true);

            if(!CameraMode)
                playerController.Init();
            
            var playerCamera = playerController.GetComponentInChildren<Camera>(true);
            if (playerCamera)
            {
                playerCamera.enabled = true;
                Camera.SetupCurrent(playerCamera);
            }
        }
        
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            
            // Draw line showing direction
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
        }
        #endif
    }
}