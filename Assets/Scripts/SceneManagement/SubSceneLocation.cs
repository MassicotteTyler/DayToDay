using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    /// <summary>
    /// A location in the scene where a subscene should be loaded.
    /// </summary>
    public class SubSceneLocation : MonoBehaviour
    {
        /// <summary>
        /// The type of subscene to load at this location.
        /// </summary>
        [SerializeField] private SubSceneType subSceneType;

        /// <summary>
        /// Flag to check if the subscene has been loaded.
        /// </summary>
        private bool _isLoaded = false;
        
        /// <summary>
        /// The type of subscene to load at this location.
        /// </summary>
        public SubSceneType SubSceneType => subSceneType;

        /// <summary>
        /// The data for the subscene.
        /// </summary>
        private SceneData _subSceneData;

        private void Start()
        {
            // Register the subscene with the scene manager
            _subSceneData = Bootstrapper.Instance.SceneLoader.GetSubSceneData(subSceneType);
        }

        private void Update()
        {
            if (_isLoaded || !_subSceneData.Reference.LoadedScene.isLoaded) return;
            MoveSubScene(_subSceneData);
            _isLoaded = true;
        }

        /// <summary>
        /// Move the subscene to the location of the SubSceneLocation object.
        /// </summary>
        /// <param name="sceneData"></param>
        public void MoveSubScene(SceneData sceneData)
        {
            if (sceneData == null)
            {
                Debug.LogWarning($"SceneData for {subSceneType} is null.");
                return;
            }

            // Move the subscene to the location
            var sceneObjects = sceneData.Reference.LoadedScene.GetRootGameObjects();

            // Move all sceneobjects so they're offset from this object.
            foreach (var sceneObject in sceneObjects)
            {
                sceneObject.transform.position = transform.position + sceneObject.transform.position;
                sceneObject.transform.rotation = transform.rotation * sceneObject.transform.rotation;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, Vector3.one);

            // Draw arrow to indicate the direction of the subscene
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.5f);
            Gizmos.DrawCube(transform.position + transform.forward * 0.5f, new Vector3(0.1f, 0.1f, 0.1f));

            Gizmos.color = Color.magenta;

            // Draw text to indicate the subscene type
            var style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.magenta;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(transform.position, subSceneType.ToString(), style);
        }
#endif
    }
}