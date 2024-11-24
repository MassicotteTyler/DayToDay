using System;
using Events;
using UI.Transition;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// HUD for the game.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class HUD : MonoBehaviour
    {
        // TODO: The following should just listen for a HUD event and not have the hud directly control them.

        private Crosshair _crosshair;
        private InteractionLabel _interactionLabel;
        private UINote _note;
        private UIWallet _wallet;
        private UIDay _day;

        private void Awake()
        {
            _crosshair = GetComponentInChildren<Crosshair>(true);
            _interactionLabel = GetComponentInChildren<InteractionLabel>(true);
            _note = GetComponentInChildren<UINote>(true);
            _wallet = GetComponentInChildren<UIWallet>(true);
            _day = GetComponentInChildren<UIDay>(true);
            UIManager.Instance.OnNodeTransitionStart += Hide;
            EndNodeEvent.OnEndNode += Hide;
            UIManager.Instance.OnNodeTransitionEnd += Show;
        }

        private void OnDestroy()
        {
            UIManager.Instance.OnNodeTransitionStart -= Hide;
            UIManager.Instance.OnNodeTransitionEnd -= Show;
            EndNodeEvent.OnEndNode -= Hide;
        }

        /// <summary>
        /// Hide the HUD.
        /// </summary>
        private void Hide()
        {
            _wallet?.gameObject.SetActive(false);
            _crosshair?.gameObject.SetActive(false);
            _interactionLabel?.gameObject.SetActive(false);
            _note?.gameObject.SetActive(false);
            _day?.gameObject.SetActive(false);
        }

        /// <summary>
        /// Show the HUD.
        /// </summary>
        private void Show()
        {
            _wallet?.gameObject.SetActive(true);
            _crosshair?.gameObject.SetActive(true);
            _day?.gameObject.SetActive(true);
        }
    }
}