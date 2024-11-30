using Bodega;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Represents the UI for the bodega job.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIBodegaJob : MonoBehaviour
    {
        /// <summary>
        /// The label for the job.
        /// </summary>
        private TextMeshProUGUI _label;
        
        /// <summary>
        /// The current job.
        /// </summary>
        private BodegaJob _currentJob;
        
        private void Start()
        {
            _label = GetComponent<TextMeshProUGUI>();
            Bodega.BodegaManager.OnJobAssigned += HandleJobAssigned;
            Bodega.BodegaManager.OnJobCompleted += HandleJobCompleted;
        }
        
        /// <summary>
        /// Handles when a job is assigned.
        /// </summary>
        /// <param name="job">The newly assigned <see cref="BodegaJob"/></param>
        private void HandleJobAssigned(BodegaJob job)
        {
            _currentJob = job;
            _currentJob.OnJobStatusChanged += UpdateLabel;
            UpdateLabel();
        }
        
        /// <summary>
        /// Handles when a job is completed.
        /// </summary>
        private void HandleJobCompleted()
        {
            _currentJob.OnJobStatusChanged -= UpdateLabel;
            _currentJob = null;
            ClearLabel();
        }
        
        private void OnDestroy()
        {
            BodegaManager.OnJobAssigned -= HandleJobAssigned;
            BodegaManager.OnJobCompleted -= HandleJobCompleted;
        }
        
        /// <summary>
        /// Updates the label with the current job status.
        /// </summary>
        private void UpdateLabel()
        {
            _label.text = $"{_currentJob.GetJobStatus()}";
        }
        
        /// <summary>
        /// Clears the label.
        /// </summary>
        private void ClearLabel()
        {
            _label.text = "";
        }
    }
}