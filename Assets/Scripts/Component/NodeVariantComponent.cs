using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using World;

namespace Component
{
    /// <summary>
    /// Component that allows the owning object to change its state based on game state conditions.
    /// </summary>
    public class NodeVariantComponent : MonoBehaviour
    {
        /// <summary>
        /// Conditions to evaluate to determine if the object should be active.
        /// </summary>
        [SerializeField] private List<GameStateCondition> Conditions;

        private void Awake()
        {
            UpdateState();

            UIManager.Instance.OnNodeTransitionEnd += UpdateState;
        }

        private void OnDestroy()
        {
            UIManager.Instance.OnNodeTransitionEnd -= UpdateState;
        }

        /// <summary>
        /// Updates the state of the object based on the conditions.
        /// </summary>
        public void UpdateState()
        {
            gameObject.SetActive(EvaluateConditions());
        }

        /// <summary>
        /// Evaluates the conditions to determine if the object should be active.
        /// </summary>
        /// <returns>True, if all conditions are met.</returns>
        public bool EvaluateConditions()
        {
            return Conditions.TrueForAll(condition => condition.Evaluate(WorldManager.Instance.GetGameState()));
        }
    }
}