using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bodega
{
    /// <summary>
    /// Represents a job to shelve items in the bodega.
    /// </summary>
    public class ShelveItemsJob : BodegaJob
    {
        /// <summary>
        /// The number of items to shelve.
        /// </summary>
        [SerializeField] private int itemsToShelveCount = 5;
        
        /// <summary>
        /// The items to shelve.
        /// </summary>
        private GameObject[] itemsToShelve;


        private void Awake()
        {
            itemsToShelve = new GameObject[itemsToShelveCount];
        }

        private void OnDestroy()
        {
           BodegaItemEvent.OnItemShelved -= OnItemShelved; 
        }

        protected override void SetupJob()
        {
            FindItemsToShelve();
            BodegaItemEvent.OnItemShelved += OnItemShelved;
        }
        
        public override void CleanupJob()
        {
            itemsToShelve = null;
            BodegaItemEvent.OnItemShelved -= OnItemShelved;
            
            // remove any remaining items
            var bodItems = GameObject.FindGameObjectsWithTag(BodegaManager.BodegaItemTag);
            foreach (var item in bodItems)
            {
                Destroy(item);
            }
        }
        
        /// <summary>
        /// Called when an item is shelved.
        /// </summary>
        /// <param name="item">The shelved item</param>
        private void OnItemShelved(GameObject item)
        {
            if (!itemsToShelve.Contains(item)) return;
            itemsToShelve = itemsToShelve.Where(i => i != item).ToArray();
            if (itemsToShelve.Length == 0)
            {
                CompleteJob();
            }
            OnJobStatusChanged?.Invoke();
            item.SetActive(false);
        }
        
        /// <summary>
        /// Finds the items to shelve.
        /// </summary>
        private void FindItemsToShelve()
        {
            // Find all items in the scene that need to be shelved.
            // For now, just find all items with the tag "ItemToShelve".
            var bodegaItems = GameObject.FindGameObjectsWithTag(BodegaManager.BodegaItemTag).ToList();
            
            if (bodegaItems.Count < itemsToShelveCount)
            {
                Debug.LogWarning("Not enough items to shelve in the scene.");
                itemsToShelveCount = bodegaItems.Count;
            }
            // Choose a random selection of items to shelve.
            for (int index = 0; index < itemsToShelveCount; index++)
            {
                var randomIndex = Random.Range(0, bodegaItems.Count);
                var item = bodegaItems[randomIndex];
                itemsToShelve[index] = item;
                bodegaItems.RemoveAt(randomIndex);
            }
            
            // Disable non-selected items.
            foreach (var item in bodegaItems)
            {
                item.SetActive(false);
            }
        }

        public override string GetJobStatus()
        {
            var itemsShelved = itemsToShelveCount - itemsToShelve.Length;
            return $"{itemsShelved}/{itemsToShelveCount} Shelved";
        }
    }
}