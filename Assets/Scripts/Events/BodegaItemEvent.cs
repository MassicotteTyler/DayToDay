using System;
using Bodega;
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
        /// <summary>
        /// Action invoked when an item is shelved.
        /// </summary>
        public static Action<GameObject> OnItemShelved;
        
        /// <summary>
        /// Action invoked when the trash is cleared.
        /// </summary>
        public static Action<GameObject> OnTrashCleared;
        
        /// <summary>
        /// The type of bodega item.
        /// </summary>
        [SerializeField] private BodegaItemType ItemType;
        
        protected override async void Execute(GameObject invoker = null)
        {
            base.Execute();
            Debug.Log("Bodega item event invoked.");
            
            if (ItemType == BodegaItemType.Stock)
            {
                OnItemShelved?.Invoke(invoker);
                
                // TODO: This could be replaced with a listener on the above action.
                WorldManager.Instance.PlayerShelvedItem();
            }
            else if (ItemType == BodegaItemType.Trash)
            {
                OnTrashCleared?.Invoke(invoker);
            }
        }
    }
}