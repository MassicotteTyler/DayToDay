using SceneManagement;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/End Node", order = 2)]
    public class EndNodeEvent : GameEvent
    {
        public override async void Invoke()
        {
            await Bootstrapper.Instance.SceneLoader.EndNode();
        }
    }
}