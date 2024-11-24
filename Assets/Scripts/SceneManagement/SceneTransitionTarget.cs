using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq.Expressions;
using SceneManagement.Transition;
using World;

namespace SceneManagement
{

    
    /// <summary>
    /// Represents a potential transition scene..
    /// </summary>
    [CreateAssetMenu(fileName = "NewSceneTransitionTarget", menuName = "Node Management/Scene Transition Target")]
    public class SceneTransitionTarget : ScriptableObject
    {
        
        /// <summary>
        /// The scene group to transition to.
        /// </summary>
        [SerializeField] public SceneGroup SceneGroup;
        
        /// <summary>
        /// The conditions to check if the transition should occur.
        /// </summary>
        [SerializeField] private GameStateCondition[] Conditions;

        /// <summary>
        /// Evaluate the conditions to see if the transition can occur.
        /// </summary>
        /// <returns>True if the conditions are met</returns>
        public bool CanTransition()
        {
            // Evaluate all conditions
            return Conditions.All(condition => condition.Evaluate(WorldManager.Instance.GetGameState()));
        }
    }
}