using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    /// <summary>
    /// Component that displays the interaction label on the screen.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class InteractionLabel : MonoBehaviour
    {
        /// <summary>
        /// Event that is triggered when the interaction label changes.
        /// </summary>
        public UnityEvent onChanged;

        /// <summary>
        /// Event that is triggered when the interaction label is shown.
        /// </summary>
        public UnityEvent onShow;

        /// <summary>
        /// Event that is triggered when the interaction label is hidden.
        /// </summary>
        public UnityEvent onHide;

        /// <summary>
        /// Label to display the interaction text.
        /// </summary>
        [SerializeField] private TextMeshProUGUI label;

        private void Start()
        {
            label = GetComponent<TextMeshProUGUI>();
            UIManager.Instance.OnInteractionLabelChanged += UpdateLabel;
            UIManager.Instance.OnHideInteractionLabel += Hide;
            Hide();
        }

        /// <summary>
        /// Update the label text.
        /// </summary>
        /// <param name="text"></param>
        private void UpdateLabel(string text)
        {
            label.text = text;
            onChanged?.Invoke();
            Show();
        }

        /// <summary>
        /// Show the interaction label.
        /// </summary>
        public void Show()
        {
            onShow?.Invoke();
            label.alpha = 1;
        }

        /// <summary>
        /// Hide the interaction label.
        /// </summary>
        public void Hide()
        {
            onHide?.Invoke();
            label.alpha = 0;
        }

        /// <summary>
        /// Unsubscribe from events when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            UIManager.Instance.OnInteractionLabelChanged -= UpdateLabel;
            UIManager.Instance.OnHideInteractionLabel -= Hide;
        }
    }
}