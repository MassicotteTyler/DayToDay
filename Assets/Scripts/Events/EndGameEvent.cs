using System;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Event for when the game ends.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/End Game", order = 4)]
    public class EndGameEvent : GameEvent
    {
        /// <summary>
        /// Action invoked when the game ends.
        /// </summary>
        public static Action<EndGameEvent> OnGameEnd;

        /// <summary>
        /// The name of the game ending.
        /// </summary>
        public string GameEndingName;
        
        /// <summary>
        /// The end game message.3
        /// </summary>
        [TextArea]
        public string EndGameMessage;
        
        /// <summary>
        /// The hint of how to achieve this ending.
        /// </summary>
        [TextArea]
        public string Hint;
        
        protected override void Execute(GameObject invoker = null)
        {
            base.Execute();
            OnGameEnd?.Invoke(this);
        }
    }
}