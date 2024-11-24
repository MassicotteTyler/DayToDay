using System;
using SceneManagement.Transition;
using UnityEngine;

namespace World
{
    /// <summary>
    /// A condition for transitioning to a scene.
    /// </summary>
    [Serializable]
    public class GameStateCondition
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
}