using System;
using UnityEngine;

namespace Events
{
    
    public interface IGameEvent
    {
        void Invoke();
    }

    /// <summary>
    /// Base class for game events.
    /// </summary>
    public abstract class GameEvent : ScriptableObject, IGameEvent
    {
        /// <summary>
        /// Invoke the event.
        /// </summary>
        public virtual async void Invoke()
        {
            Debug.Log($"Event |{name}| invoked.");
            EventBus.Instance.OnEventTriggered?.Invoke(this);
        }
    }
}