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
        /// Initialize the UIManager.
        /// </summary>
        protected override void Initialize()
        {
            Debug.Log("UIManager initialized");
        }
    }
}