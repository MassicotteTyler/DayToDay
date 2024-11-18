using System;
using UnityEngine;

namespace Events
{
    
    public interface IGameEvent
    {
        void Invoke(GameObject invoker = null);
    }

    /// <summary>
    /// Base class for game events.
    /// </summary>
    public abstract class GameEvent : ScriptableObject, IGameEvent
    {
        /// <summary>
        /// Invoke the event.
        /// <param name="invoker">The GameObject that invoked the event.</param>
        /// </summary>
        public virtual async void Invoke(GameObject invoker = null)
        {
            Debug.Log($"Event |{name}| invoked.");
            EventBus.Instance.OnEventTriggered?.Invoke(this);
        }
    }
}