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
    /// A condition for transitioning to a scene.
    /// </summary>
    [Serializable]
    public class SceneTransitionCondition
    {
        /// <summary>
        /// The expression to evaluate.
        /// </summary>
        [TextArea]
        [SerializeField]
        public string Expression;

        /// <summary>
        /// Evaluates the condition.
        /// </summary>
        /// <param name="state">WorldState to evaluate on</param>
        /// <returns></returns>
        public bool Evaluate(WorldManager.GameState state)
        {
            try
            {
                return new StateExpressionEvaluator(state.Player, state.World).Evaluate(Expression);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error evaluating expression: {Expression}");
                Debug.LogError(e);
                return false;
            }
        }
        
    }
    
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
        [SerializeField] private SceneTransitionCondition[] Conditions;

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