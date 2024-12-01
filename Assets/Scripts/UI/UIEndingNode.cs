using System;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace UI
{
    /// <summary>
    /// Represents a possible ending node in the game.
    /// </summary>
    public class UIEndingNode : MonoBehaviour
    {
        /// <summary>
        /// Event for when the player clicks on the ending node.
        /// </summary>
        public static event Action<string> OnEndGameDescriptionEvent;
        
        /// <summary>
        /// The end game event.
        /// </summary>
        [SerializeField] private EndGameEvent _endGameEvent;
        
        /// <summary>
        /// The end game message.
        /// </summary>
        [SerializeField] private TextMeshProUGUI _endText;
        
        /// <summary>
        /// The color of the node if it's the current ending.
        /// </summary>
        [SerializeField] private Color _activeColor = Color.green;
        
        /// <summary>
        /// The image of the node.
        /// </summary>
        private Image _Image;
        
        /// <summary>
        /// The initial color of the image.
        /// </summary>
        private Color _initialImageColor; 
        
        private void Start()
        {
            _endText = GetComponentInChildren<TextMeshProUGUI>();
            _Image = GetComponent<Image>();
            _initialImageColor = _Image?.color ?? Color.white;
            UpdateEndText();
        }

        public void OnMouseDown()
        {
            var descriptionText = PlayerManager.Instance.EndGameEvents.ContainsKey(_endGameEvent.GameEndingName) ?
                _endGameEvent.EndGameMessage :
               $"Hint: {_endGameEvent.Hint}";
            OnEndGameDescriptionEvent?.Invoke(descriptionText);
        }

        /// <summary>
        /// Updates the end text.
        /// </summary>
        private void UpdateEndText()
        {
            if (_endGameEvent && PlayerManager.Instance.EndGameEvents.ContainsKey(_endGameEvent.GameEndingName))
            {
                _endText.text = _endGameEvent.GameEndingName;
                
                if (!_Image) return;
                _Image.color = PlayerManager.Instance.CurrentEndGameEvent == _endGameEvent ? _activeColor : _initialImageColor;
            }
            else
            {
                _endText.text = "???";
            }
        } 
    }
}