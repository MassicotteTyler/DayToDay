using System;
using SceneManagement;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/End Node", order = 2)]
    public class EndNodeEvent : GameEvent
    {
        public static Action OnEndNode;
        protected override async void Execute(GameObject invoker = null)
        {
            base.Execute();
            OnEndNode?.Invoke();
            Bootstrapper.Instance.SceneLoader.EndNode();
        }
    }
}