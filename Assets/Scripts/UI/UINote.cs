using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Component that displays the note on the screen.
    /// </summary>
    public class UINote : MonoBehaviour
    {
        /// <summary>
        /// Background of the note.
        /// </summary>
        [SerializeField] private Image background;
        
        /// <summary>
        /// Image of the note.
        /// </summary>
        [SerializeField] private Image noteImage;
        
        /// <summary>
        /// Text of the note.
        /// </summary>
        [SerializeField] private TextMeshProUGUI noteTextUI;

        private void Start()
        {
            background = GetComponent<Image>();

            if (!noteImage)
            {
                noteImage = GetComponentInChildren<Image>();
            }

            if (!noteTextUI)
            {
                noteTextUI = GetComponentInChildren<TextMeshProUGUI>();
            }

            UIManager.Instance.OnNoteTextChange += UpdateNote;
            Hide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.OnExitUIMode?.Invoke();
            }
        }

        /// <summary>
        /// Show the note.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            UIManager.Instance.HideActiveUI += Hide;
            UIManager.Instance.OnEnterUIMode?.Invoke();
            UIManager.Instance.OnExitUIMode += Hide;
        }

        /// <summary>
        /// Hide the note.
        /// </summary>
        public void Hide()
        {
            UIManager.Instance.HideActiveUI -= Hide;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Update the note text.
        /// </summary>
        /// <param name="text">Text to update the UI with</param>
        public void UpdateNote(string text)
        {
            noteTextUI.text = text;
            Show();
        }

        private void OnDestroy()
        {
            UIManager.Instance.OnNoteTextChange -= UpdateNote;
            UIManager.Instance.HideActiveUI -= Hide;
        }
    }
}