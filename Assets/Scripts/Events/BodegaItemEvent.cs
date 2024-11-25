using UnityEngine;
using World;

namespace Events
{
    /// <summary>
    /// Event for when a bodega item is interacted with.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Bodega Item", order = 3)]
    public class BodegaItemEvent : GameEvent 
    {
        protected override async void Execute(GameObject invoker = null)
        {
            base.Execute();
            Debug.Log("Bodega item event invoked.");
            WorldManager.Instance.PlayerShelvedItem();
            Destroy(invoker);
        }
    }
}