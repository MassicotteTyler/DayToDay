using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Seen Green Man", order = 5)]
    public class SeenGreenManEvent : GameEvent
    {
        /// <summary>
        /// Action to be invoked upon seeing the green man.
        /// </summary>
        public static Action seenGreenMan;
        
        protected override void Execute(GameObject invoker = null)
        {
            seenGreenMan?.Invoke();
        }
    }
}