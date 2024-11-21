using System;
using SceneManagement;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/End Node", order = 2)]
    public class EndNodeEvent : GameEvent
    {
        public static Action OnEndNode;
        public override async void Invoke(GameObject invoker = null)
        {
            base.Invoke();
            OnEndNode?.Invoke();
            await Bootstrapper.Instance.SceneLoader.EndNode();
        }
    }
}