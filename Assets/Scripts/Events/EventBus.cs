using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Events
{
    // Modify the EventBus class
    public class EventBus : Singleton<EventBus>
    {

        public Action<IGameEvent> OnEventTriggered;
        
        protected override void Initialize()
        {
            Debug.Log("EventBus initialized");
        }
    }
}