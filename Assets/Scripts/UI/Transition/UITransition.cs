using System;
using System.Collections;
using UnityEngine;

namespace UI.Transition
{
    /// <summary>
    /// UI transition component to handle transitions between nodes.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class UITransition : MonoBehaviour
    {
        /// <summary>
        /// The CanvasRenderer attached to this GameObject.
        /// </summary>
        private CanvasRenderer _canvasRenderer;
        
        /// <summary>
        /// The Coroutine used to handle the transition.
        /// </summary>
        private Coroutine _transitionCoroutine;
        
        /// <summary>
        /// The duration of the transition.
        /// </summary>
        [SerializeField] private float transitionDuration = 1.5f;
        private void Awake()
        {
           _canvasRenderer = GetComponent<CanvasRenderer>(); 
           
           UIManager.Instance.OnNodeTransitionStart += TransitionStart;
           UIManager.Instance.OnNodeTransitionEnd += TransitionEnd;
        }

        private void OnDestroy()
        {
            UIManager.Instance.OnNodeTransitionStart -= TransitionStart;
            UIManager.Instance.OnNodeTransitionEnd -= TransitionEnd;
            StopAllCoroutines();
        }

        /// <summary>
        /// Start the transition.
        /// </summary>
        private void TransitionStart()
        {
            if (_transitionCoroutine != null)
                StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = StartCoroutine(Transition(0f, 1f));
        }

        /// <summary>
        /// End the transition.
        /// </summary>
        private void TransitionEnd()
        {
            if (_transitionCoroutine != null)
                StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = StartCoroutine(Transition(1f, 0f));
        }

        /// <summary>
        /// Coroutine to handle the transition.
        /// </summary>
        /// <param name="startingAlpha">Initial value of the Alpha</param>
        /// <param name="targetAlpha">Target end value of the Alpha</param>
        /// <returns></returns>
        private IEnumerator Transition(float startingAlpha, float targetAlpha)
        {
            var time = 0f;
            UIManager.Instance.TransitionStart();
            while (!_canvasRenderer.GetAlpha().Equals(targetAlpha))
            {
                time += Time.deltaTime;
                var delta = Mathf.Clamp(time / transitionDuration, 0f, 1f);
                var newAlpha = Mathf.Lerp(startingAlpha, targetAlpha, delta);
                _canvasRenderer.SetAlpha(newAlpha);
                yield return null;
            }
            UIManager.Instance.TransitionEnd();
        }
    }
}