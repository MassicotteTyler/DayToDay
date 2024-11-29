using System;
using UnityEngine;
using World;

namespace SceneManagement.Transition
{
    /// <summary>
    /// Evaluates state expressions.
    /// </summary>
    public readonly struct StateExpressionEvaluator
    {
        /// <summary>
        /// The player state.
        /// </summary>
        private readonly PlayerState player;
        /// <summary>
        /// The world state.
        /// </summary>
        private readonly WorldState world;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StateExpressionEvaluator"/> struct.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="world"></param>
        public StateExpressionEvaluator(PlayerState player, WorldState world)
        {
            this.player = player;
            this.world = world;
        }
        
        /// <summary>
        /// Evaluates the expression.
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>If the condition is true</returns>
        public bool Evaluate(string expression)
        {
            if (string.IsNullOrEmpty(expression)) return true;
            
            // Split by AND
            var andConditions = expression.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            
            // Evaluate each AND condition
            foreach (var condition in andConditions)
            {
                // Split by OR
                var orConditions = condition.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                
                
                // if any Or condition is true, this AND group is true
                bool orResullt = false;
                foreach (var orCondition in orConditions)
                {
                    orResullt = EvaluteSimpleCondition(orCondition.Trim());
                    if (orResullt) break;
                }
                
                // If this aND group is false, the whole expression is false
                if (!orResullt) return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Evaluates a simple condition. Expects the format: "World.Day > 3"
        /// </summary>
        /// <param name="expression">The condition to evaluate</param>
        /// <returns>If this condition is met</returns>
        private bool EvaluteSimpleCondition(string expression)
        {
            // Split into parts: "World.Day > 3" -> ["World.Day", ">", "3"]
            var parts = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                Debug.LogError($"Invalid expression: {expression}");
                return false;
            }

            var propertyPath = parts[0].Split('.');
            if (propertyPath.Length != 2)
            {
                Debug.LogError($"Invalid property path: {parts[0]}");
                return false;
            }
            
            var value = GetPropertyValue(propertyPath[0], propertyPath[1]);
            if (value == null)
            {
                Debug.LogError($"Invalid property path: {parts[0]}");
                return false;
            }
            
            // check if we're comparing to another field ex Player.Money
            object compareValue;
            if (parts[2].Contains("."))
            {
                var comparePropertyPath = parts[2].Split('.');
                if (comparePropertyPath.Length != 2)
                {
                    Debug.LogError($"Invalid property path: {parts[2]}");
                    return false;
                }
                
                compareValue = GetPropertyValue(comparePropertyPath[0], comparePropertyPath[1]);
                if (compareValue == null)
                {
                    Debug.LogError($"Invalid property path: {parts[2]}");
                    return false;
                }        
            }
            else
            {
                // Otherwise, parse the value
                compareValue = ParseValue(parts[2], value.GetType());
            }
            
            // Compare based on the operator
            return CompareValues((IComparable) value, parts[1], (IComparable) compareValue);
        }

        /// <summary>
        /// Gets the value of a property from a state.
        /// </summary>
        /// <param name="stateName">Name of the state object to access</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private object GetPropertyValue(string stateName, string propertyName)
        {
            switch (stateName.ToLower())
            {
                case "player":
                    return typeof(PlayerState).GetProperty(propertyName)?.GetValue(player);
                case "world":
                    var field = typeof(WorldState).GetProperty(propertyName);
                    if (field == null)
                    {
                        Debug.LogError($"Invalid property name: {propertyName}");
                        return null;
                    }
                    return typeof(WorldState).GetProperty(propertyName)?.GetValue(world);
                default:
                    Debug.LogError($"Invalid state name: {stateName}");
                    return null;
            }
        }

        /// <summary>
        /// Parses a string value to a target type.
        /// </summary>
        /// <param name="value">Value to parse</param>
        /// <param name="targetType">The type to parse into</param>
        /// <returns></returns>
        private object ParseValue(string value, Type targetType)
        {
            // Get parse method from targetType
            var parseMethod = targetType.GetMethod("Parse", new[] { typeof(string) });
            if (parseMethod == null)
            {
                Debug.LogError($"No parse method found for type: {targetType}");
                return null;
            }
            
            return parseMethod.Invoke(null, new object[] { value });
        }
        
        /// <summary>
        /// Compares two values based on an operator.
        /// </summary>
        /// <param name="left">Left value to compare</param>
        /// <param name="op">Comparison operation</param>
        /// <param name="right">Right value to compare</param>
        /// <returns></returns>
        private bool CompareValues(IComparable left, string op, IComparable right)
        {
            // Ensure the comparables are of the same type
            if (left.GetType() != right.GetType())
            {
                Debug.LogWarning($"Trying to compare different types: {left.GetType()} and {right.GetType()}");
                right = (IComparable)Convert.ChangeType(right, left.GetType());
            }
            
            switch (op)
            {
                case ">":
                    return left.CompareTo(right) > 0;
                case "<":
                    return left.CompareTo(right) < 0;
                case ">=":
                    return left.CompareTo(right) >= 0;
                case "<=":
                    return left.CompareTo(right) <= 0;
                case "==":
                    return left.CompareTo(right) == 0;
                case "!=":
                    return left.CompareTo(right) != 0;
                default:
                    Debug.LogError($"Invalid operator: {op}");
                    return false;
            }
        }
    }
}