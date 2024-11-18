using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneLighting
{
    /// <summary>
    /// Script to be put on a trigger collider in the scene.
    /// Will change ambient intensity on trigger Enter
    /// </summary>
    public class LightSettingTrigger : MonoBehaviour
    {
        /// <summary>
        /// The lighting mode to set when OnTriggerEnter is called
        /// </summary>
        public LightingMode LightingMode = LightingMode.Default;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                SceneLightingManager.Instance?.LerpLightingMode(LightingMode);
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            BoxCollider col = GetComponent<BoxCollider>();

            if(col != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            }
        }
        #endif

    }

}
