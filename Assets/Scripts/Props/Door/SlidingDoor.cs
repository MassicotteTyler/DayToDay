using System.Collections;
using UnityEngine;

namespace Props.Door
{
    /// <summary>
    /// A component that represents a sliding door which can be interacted with.
    /// <p>Opens by moving to the target position.</p>
    /// </summary>
    public class SlidingDoor : BaseDoor
    {
        /// <summary>
        /// Transform representing the open location of the door.
        /// </summary>
        [Header("Sliding Door Settings")]
        [SerializeField] private Transform openTransform;
        
        /// <summary>
        /// The position of the door when it is open.
        /// </summary>
        private Vector3 openPosition;
        
        /// <summary>
        /// The position of the door when it is closed.
        /// </summary>
        private Vector3 closePosition;
        
        private void Awake()
        {
            openPosition = openTransform.position;
            closePosition = transform.position;
        }

        /// <summary>
        /// Coroutine for moving the door to the target position.
        /// </summary>
        /// <param name="targetPosition">The destination of the door movement.</param>
        /// <returns></returns>
        private IEnumerator MoveDoor(Vector3 targetPosition)
        {
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
        }

        protected override void OpenDoorTransition()
        {
            if (moveDoorCoroutine != null)
            {
                StopCoroutine(moveDoorCoroutine);
            }
            moveDoorCoroutine = StartCoroutine(MoveDoor(openPosition));
        }

        protected override void CloseDoorTransition()
        {
            if (moveDoorCoroutine != null)
            {
                StopCoroutine(moveDoorCoroutine);
            }
            moveDoorCoroutine = StartCoroutine(MoveDoor(closePosition));
        }
    }
}