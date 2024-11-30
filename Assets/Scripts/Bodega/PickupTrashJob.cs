using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Bodega
{
    /// <summary>
    /// Represents a job to shelve items in the bodega.
    /// </summary>
    public class PickupTrashJob : BodegaJob
    {
        /// <summary>
        /// The number of trash items to clear.
        /// </summary>
        [SerializeField] private int trashToClean = 2;
        
        /// <summary>
        /// The trash to clear.
        /// </summary>
        private GameObject[] trashToClear;


        private void Awake()
        {
            trashToClear = new GameObject[trashToClean];
        }

        private void OnDestroy()
        {
           BodegaItemEvent.OnTrashCleared -= OnTrashCleared; 
        }

        protected override void SetupJob()
        {
            FindTrashToPickup();
            BodegaItemEvent.OnTrashCleared += OnTrashCleared;
        }
        
        public override void CleanupJob()
        {
            trashToClear = null;
            BodegaItemEvent.OnTrashCleared -= OnTrashCleared;
            
            // remove any remaining items
            var bodItems = GameObject.FindGameObjectsWithTag(BodegaManager.BodegaTrashTag);
            foreach (var item in bodItems)
            {
                Destroy(item);
            }
        }
        
        /// <summary>
        /// Called when an trash item is cleared
        /// </summary>
        /// <param name="item">The cleared item</param>
        private void OnTrashCleared(GameObject item)
        {
            if (!trashToClear.Contains(item)) return;
            trashToClear = trashToClear.Where(i => i != item).ToArray();
            if (trashToClear.Length == 0)
            {
                CompleteJob();
            }
            OnJobStatusChanged?.Invoke();
            item.SetActive(false);
        }
        
        /// <summary>
        /// Finds the trash items to clear.
        /// </summary>
        private void FindTrashToPickup()
        {
            // Find all items in the scene that need to be cleared.
            // For now, just find all items with the tag "BodegaTrash".
            var bodegaItems = GameObject.FindGameObjectsWithTag(BodegaManager.BodegaTrashTag).ToList();
            
            if (bodegaItems.Count < trashToClean)
            {
                Debug.LogWarning("Not enough trash to clean in the scene.");
                trashToClean = bodegaItems.Count;
            }
            // Choose a random selection of items to clear
            for (int index = 0; index < trashToClean; index++)
            {
                var randomIndex = Random.Range(0, bodegaItems.Count);
                var item = bodegaItems[randomIndex];
                trashToClear[index] = item;
                bodegaItems.RemoveAt(randomIndex);
            }
            
            // Disable non-selected items.
            foreach (var item in bodegaItems)
            {
                item.SetActive(false);
            }
            OnJobStatusChanged?.Invoke();
        }

        public override string GetJobStatus()
        {
            var itemsShelved = trashToClean - trashToClear.Length;
            return $"{itemsShelved}/{trashToClean} cleared";
        }
    }
}