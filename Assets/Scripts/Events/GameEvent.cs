using System;
using System.Collections.Generic;
using UnityEngine;
using World;

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
        /// Conditions that must be met for the event to be Executed.
        /// </summary>
        [SerializeField] private List<GameStateCondition> Conditions;
        
        /// <summary>
        /// Invoke the event.
        /// <param name="invoker">The GameObject that invoked the event.</param>
        /// </summary>
        public void Invoke(GameObject invoker = null)
        {
            Debug.Log($"Event |{name}| invoked.");
            if (!EvaluateConditions())
            {
                Debug.Log($"Event |{name}| conditions not met.");
                return;
            }
            EventBus.Instance.OnEventTriggered?.Invoke(this);
            Execute(invoker);
        }

        /// <summary>
        /// Execute the event.
        /// </summary>
        /// <param name="invoker">The <see cref="GameObject"/> that executed the event.</param>
        protected virtual void Execute(GameObject invoker = null)
        {
            Debug.Log($"Event |{name}| Executed.");
        }
        
        /// <summary>
        /// Evaluates the conditions to determine if the event should be executed.
        /// </summary>
        /// <returns>True, if all conditions are met.</returns>
        protected bool EvaluateConditions()
        {
            return Conditions?.TrueForAll(condition => condition.Evaluate(WorldManager.Instance.GetGameState())) ?? true;
        }
    }
}