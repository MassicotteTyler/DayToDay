using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bodega
{
    
    /// <summary>
    /// The type of item in the bodega.
    /// </summary>
    public enum BodegaItemType
    {
        /// <summary>
        /// The item is shelve stock.
        /// </summary>
        Stock,
        /// <summary>
        /// The item is trash.
        /// </summary>
        Trash,
    }
    
    /// <summary>
    /// Manager for the bodega.
    /// </summary>
    public class BodegaManager : Utility.Singleton<BodegaManager> 
    {
        /// <summary>
        /// The tag for bodega items.
        /// </summary>
        public static string BodegaItemTag = "BodegaItem";
        public static string BodegaTrashTag = "BodegaTrash";
        
        /// <summary>
        /// Action invoked when a job is assigned.
        /// </summary>
        public static Action<BodegaJob> OnJobAssigned;
        
        /// <summary>
        /// Action invoked when a job is completed.
        /// </summary>
        public static Action OnJobCompleted;
        
        /// <summary>
        /// The current job being worked on.
        /// </summary>
        public BodegaJob currentJob { get; private set; } = null;

        /// <summary>
        /// The list of completed jobs.
        /// </summary>
        private List<BodegaJob> completedJobs = new();

        private void Start()
        {
           AssignNewJob();
           
           OnJobCompleted += HandleJobCompleted;
           UIManager.Instance.OnNodeTransitionEnd += AssignNewJob;
        }

        private void OnDestroy()
        {
           OnJobCompleted -= HandleJobCompleted; 
        }

        /// <summary>
        /// Handles when a job is completed.
        /// </summary>
        private void HandleJobCompleted()
        {
            completedJobs.Add(currentJob);
        }
        
        /// <summary>
        /// Gets the list of completed jobs.
        /// </summary>
        /// <returns>List of completed <see cref="BodegaJob"/>'s</returns>
        public List<BodegaJob> GetCompletedJobs()
        {
            return completedJobs;
        }

        /// <summary>
        /// Finds and assigns a <see cref="BodegaJob"/> to the player.
        /// </summary>
        private void AssignNewJob()
        {
            // Find available jobs
            var availableJobs = FindObjectsOfType<BodegaJob>().ToList();
            if (availableJobs.Count == 0) return;
            
            // Just random from available jobs for now,
            // TODO: Should jobs be repeatable?
            currentJob = availableJobs[Random.Range(0, availableJobs.Count)];
            OnJobAssigned?.Invoke(currentJob);
            currentJob.StartJob();
            
            // TODO: Should we support more than one job a day?
            // Disable other jobs
            foreach (var job in availableJobs.Where(j => j != currentJob))
            {
                job.CleanupJob();
                job.enabled = false;
            }
        }
    }
}