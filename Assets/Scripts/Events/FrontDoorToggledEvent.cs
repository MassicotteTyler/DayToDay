using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/FrontDoorToggled", order = 6)]
    public class FrontDoorToggledEvent : GameEvent
    {
        /// <summary>
        /// Action to be invoked opening the front door.
        /// </summary>
        public static Action frontDoorToggled;
        
        protected override void Execute(GameObject invoker = null)
        {
            frontDoorToggled?.Invoke();
        }
    }
}