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
        private void Awake()
        {
           StartCoroutine(WaitForPlayerController());
           UIManager.Instance.OnNodeTransitionEnd += MovePlayerToSpawn;
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
            if (!PlayerManager.Instance.PlayerController)
            {
                Debug.LogError("PlayerController not found.");
                return;
            }
           
            // Move the player to the spawn point.
            
            PlayerManager.Instance.PlayerController.transform.SetPositionAndRotation(transform.position, transform.rotation);
            PlayerManager.Instance.PlayerController.MovementEnabled = true;
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