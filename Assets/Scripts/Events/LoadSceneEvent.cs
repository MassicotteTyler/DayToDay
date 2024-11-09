using SceneManagement;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Event for loading a scene group.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Load Scene Event", order = 1)]
    public class LoadSceneEvent : GameEvent
    {
        /// <summary>
        /// The name of the event.
        /// </summary>
        public string EventName = "Load Scene";

        /// <summary>
        /// The scene group to be loaded.
        /// </summary>
        public SceneGroup SceneGroup;
        
        /// <summary>
        /// Invokes the event to load the specified scene group.
        /// </summary>
        public override async void Invoke()
        {
            if (!SceneGroup)
            {
                Debug.LogError($"Event Error |{EventName}|: SceneGroup is null.");
                return;
            }
            await Bootstrapper.Instance.SceneLoader.LoadSceneGroup(SceneGroup);
        }
    }
}