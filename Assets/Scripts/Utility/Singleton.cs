using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utility
{
    /// <summary>
    /// A generic singleton class for Unity components.
    /// </summary>
    /// <typeparam name="T">The type of the singleton component.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : UnityEngine.Component
    {
        /// <summary>
        /// If set to <c>true</c>, the object will be unparented on Awake.
        /// </summary>
        public bool UnParentOnAwake = true;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        protected static T instance;

        /// <summary>
        /// Gets the singleton instance, creating it if necessary.
        /// </summary>
        public static T Instance => instance ??= SingletonGameObject.AddComponent<T>();

        /// <summary>
        /// Gets the GameObject for the singleton instance, creating it if necessary.
        /// </summary>
        private static GameObject SingletonGameObject
        {
            get
            {
                if (instance != null)
                {
                    return instance.gameObject;
                }

                var go = new GameObject(typeof(T).Name);
                return go;
            }
        }

        /// <summary>
        /// Unity's Awake method. Initializes the singleton.
        /// </summary>
        protected void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the singleton instance.
        /// </summary>
        protected virtual void Initialize()
        {
            if (!Application.isPlaying) return;

            if (UnParentOnAwake)
            {
                transform.SetParent(null);
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                enabled = true;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}