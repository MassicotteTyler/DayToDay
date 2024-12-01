using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using SceneManagement;
using TMPro;
using UI;
using World;

namespace UI
{
    /// <summary>
    /// UI for the end of the game.
    /// </summary>
    public class UIEnd : MonoBehaviour
    {
        /// <summary>
        /// This GameEvent will be invoked by Start Game's onClick
        /// </summary>
        [Tooltip("Triggered on Start Game button press")]
        public Events.GameEvent LoadSceneEvent;
        
        /// <summary>
        /// The text to display at the end of the game.
        /// </summary>
        [SerializeField] private TextMeshProUGUI _endText;

        /// <summary>
        /// The name of the game ending.
        /// </summary>
        [SerializeField] private TextMeshProUGUI _endTitleText;
        
        /// <summary>
        /// The panel that displays the endings.
        /// </summary>
        [SerializeField] private GameObject _endingsPanel;
        
        /// <summary>
        /// The text to display the ending description.
        /// </summary>
        [SerializeField] private TextMeshProUGUI _endingDescriptionText;

        private void Start()
        {
            UIManager.Instance.OnNodeTransitionEnd += EnableCursor;
            UpdateEndText();
            
            UIEndingNode.OnEndGameDescriptionEvent += UpdateEndingDescription;
        }

        /// <summary>
        /// Updates the end text.
        /// </summary>
        private void UpdateEndText()
        {
            var gameEndEvent = PlayerManager.Instance.CurrentEndGameEvent;
            if (gameEndEvent)
            {
                _endText.text = gameEndEvent.EndGameMessage;
                _endTitleText.text = gameEndEvent.GameEndingName;
            }
        }
        
        /// <summary>
        /// Updates the ending description.
        /// </summary>
        /// <param name="description">Description of the ending</param>
        private void UpdateEndingDescription(string description)
        {
            _endingDescriptionText.text = description;
        }

        private void OnDestroy()
        {
            UIManager.Instance.OnNodeTransitionEnd -= EnableCursor;
        }

        void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// Invoke a GameEvent. 
        /// This function is called onClick
        /// </summary>
        public void ButtonPress_Start()
        {
            WorldManager.Instance?.ResetGameState();
            PlayerManager.Instance.IsGameEnded = false;
            LoadSceneEvent?.Invoke();
        }

        /// <summary>
        /// Toggles the endings panel.
        /// </summary>
        public void ToggleEndings()
        {
            _endingsPanel?.SetActive(!_endingsPanel?.activeSelf ?? false);
        }

        /// <summary>
        /// Exits the application. On the Web platform, Application.Quit stops the Web Player but doesn't affect the web page front end.
        /// Mainly used for desktop builds.
        /// 
        /// This function is called onClick
        /// </summary>
        public void ButtonPress_Exit()
        {
            Application.Quit();
        }
    }
}