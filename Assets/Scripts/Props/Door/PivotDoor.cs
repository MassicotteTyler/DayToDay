using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Props.Door
{
    /// <summary>
    /// A component that represents a pivot door which can be interacted with.
    /// <p>Opens by rotating around a pivot point.</p>
    /// </summary>
    public class PivotDoor : BaseDoor
    {
        /// <summary>
        /// The angle at which the door is open.
        /// </summary>
        [Header("Pivot Door Settings")]
        [SerializeField] private float openAngle = 90f;

        /// <summary>
        /// The angle at which the door is closed.
        /// </summary>
        [SerializeField] private float closeAngle = 0f;
        
        /// <summary>
        /// The transform around which the door pivots.
        /// </summary>
        [SerializeField] private Transform pivotTransform;
        
        protected override void OpenDoorTransition()
        {
            if (moveDoorCoroutine != null)
            {
                StopCoroutine(moveDoorCoroutine);
            }
            moveDoorCoroutine = StartCoroutine(Pivot(closeAngle, openAngle, speed));
        }
        
        protected override void CloseDoorTransition()
        {
            if (moveDoorCoroutine != null)
            {
                StopCoroutine(moveDoorCoroutine);
            }
            moveDoorCoroutine = StartCoroutine(Pivot(openAngle, closeAngle, speed));
        }
        
        /// <summary>
        /// Coroutine for transitioning the door between open and closed states.
        /// </summary>
        /// <param name="startAngle">The starting angle of the door.</param>
        /// <param name="endAngle">The ending angle of the door.</param>
        /// <param name="duration">The duration of the transition.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator Pivot(float startAngle, float endAngle, float duration)
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
    }
}