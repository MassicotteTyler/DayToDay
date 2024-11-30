using System;
using UnityEngine;

namespace Bodega
{
    /// <summary>
    /// Represents a job that can be done in the bodega. 
    /// </summary>
    public abstract class BodegaJob : MonoBehaviour
    {
        /// <summary>
        /// The description of the job.
        /// </summary>
        [SerializeField] public string JobDescription;
        
        /// <summary>
        /// Triggered when the job status changes.
        /// </summary>
        public Action OnJobStatusChanged;
        
        /// <summary>
        /// Starts the job.
        /// </summary>
        public void StartJob()
        {
           SetupJob();
        }
        
        /// <summary>
        /// Completes the job.
        /// </summary>
       public void CompleteJob()
       {
           BodegaManager.OnJobCompleted?.Invoke();
           CleanupJob();
       } 
       
       /// <summary>
       /// Sets up the job.
       /// </summary>
       protected virtual void SetupJob()
       {
           
       }

       /// <summary>
       /// Cleans up the job. Used to remove any listeners or references.
       /// </summary>
       public virtual void CleanupJob()
       {
           
       }

       /// <summary>
       /// Gets the job description.
       /// </summary>
       /// <returns>Description of the job</returns>
       public virtual string GetJobDescription()
       {
           return JobDescription;
       }

       /// <summary>
       /// Gets the job status.
       /// </summary>
       /// <returns>Current status of  the job</returns>
       public virtual string GetJobStatus()
       {
           return "";
       }
    }
}