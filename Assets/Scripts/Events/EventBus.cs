using System;
using System.Collections.Generic;
using SceneManagement;
using UnityEngine;
using Utility;

namespace Events
{
    // Modify the EventBus class
    public class EventBus : Singleton<EventBus>
    {
        public Action<IGameEvent> OnEventTriggered;
    }
}