using System;
using UnityEngine;
using Utility;

namespace UI
{
    /// <summary>
    /// Manager for UI elements in the game.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// Action to trigger the interaction label to change.
        /// </summary>
        public Action<string> OnInteractionLabelChanged;

        /// <summary>
        /// Action to trigger the interaction label to hide.
        /// </summary>
        public Action OnHideInteractionLabel;
        
        /// <summary>
        /// Action to trigger the note ui to change.
        /// </summary>
        public Action<string> OnNoteTextChange;

        /// <summary>
        /// Action to trigger the note ui to hide.
        /// </summary>
        public Action OnHideNote;

        /// <summary>
        /// Action to trigger the active UI to hide.
        /// </summary>
        public Action HideActiveUI;

        /// <summary>
        /// Action to trigger the UI to enter UI mode.
        /// </summary>
        public Action OnEnterUIMode;
        
        /// <summary>
        /// Action to trigger the UI to exit UI mode.
        /// </summary>
        public Action OnExitUIMode;
        
        /// <summary>
        /// Initialize the UIManager.
        /// </summary>
        protected override void Initialize()
        {
            Debug.Log("UIManager initialized");
        }
    }
}