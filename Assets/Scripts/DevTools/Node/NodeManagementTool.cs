using System;
using UnityEditor;
using UnityEngine;

namespace DevTools.Node
{
    /// <summary>
    /// Configuration for the Node Management Tool
    /// </summary> 
    public struct NodeManagementConfig
    {
        /// <summary>
        /// Enable the bootstrapper
        /// </summary>
        public bool enableBootstrapper;

        /// <summary>
        /// The player prefab to spawn
        /// </summary>
        public GameObject playerPrefab;

        /// <summary>
        /// The path to the player prefab
        /// </summary>
        public string playerPrefabPath;

        /// <summary>
        /// The position to spawn the player
        /// </summary>
        public Vector3 playerSpawnPosition;

        /// <summary>
        /// The rotation to spawn the player
        /// </summary>
        public Quaternion playerSpawnRotation;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Tool for managing nodes in the scene
    /// </summary>
    public class NodeManagementTool : EditorWindow
    {
        /// <summary>
        /// Configuration for the tool
        /// </summary>
        public static NodeManagementConfig config;

        /// <summary>
        /// Path to the settings file
        /// </summary>
        private const string DEV_SETTINGS_PATH = "Assets/Editor/DevTools/Node/NodeManagementTool.json";

        /// <summary>
        /// Load settings and show the window
        /// </summary>
        [MenuItem("Window/Node Management Tool")]
        public static void ShowWindow()
        {
            LoadSettings();
            GetWindow<NodeManagementTool>("Node Management");
        }

        private void Awake()
        {
            LoadSettings();
        }

        private void OnEnable()
        {
            LoadSettings();
        }

        /// <summary>
        /// Save the configuration when the inspector updates
        /// </summary>
        private void OnInspectorUpdate()
        {
            if (Application.isPlaying) return;
            SaveConfig();
        }

        private void Update()
        {
            if (Application.isPlaying) return;
            if (SceneView.lastActiveSceneView != null)
            {
                config.playerSpawnPosition = SceneView.lastActiveSceneView.camera.transform.position;
                config.playerSpawnRotation = SceneView.lastActiveSceneView.camera.transform.localRotation;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Node Management", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // Config
            config.enableBootstrapper = EditorGUILayout.Toggle("Enable Bootstrapper", config.enableBootstrapper);

            // Player Prefab
            config.playerPrefab = (GameObject)EditorGUILayout.ObjectField("Player Prefab", config.playerPrefab,
                typeof(GameObject), allowSceneObjects: false);
            config.playerPrefabPath = AssetDatabase.GetAssetPath(config.playerPrefab);

            // Detect changes and then save
            if (GUI.changed)
            {
                SaveConfig();
            }

            // Load Settings button
            if (GUILayout.Button("Load Settings"))
            {
                LoadSettings();
            }

            if (GUILayout.Button("Spawn player"))
            {
                Instantiate(config.playerPrefab, config.playerSpawnPosition, config.playerSpawnRotation);
            }
        }

        /// <summary>
        /// Load the settings from the configuration file. Runs on startup.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void LoadSettings()
        {
            if (System.IO.File.Exists(DEV_SETTINGS_PATH))
            {
                var json = System.IO.File.ReadAllText(DEV_SETTINGS_PATH);
                config = JsonUtility.FromJson<NodeManagementConfig>(json);
                config.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(config.playerPrefabPath);
            }
            else
            {
                config = new NodeManagementConfig { enableBootstrapper = true, playerPrefab = null };
            }
        }

        /// <summary>
        /// Save the configuration to the configuration file
        /// </summary>
        private static void SaveConfig()
        {
            var json = JsonUtility.ToJson(config, prettyPrint: true);

            // Ensure directory exists
            var directory = System.IO.Path.GetDirectoryName(DEV_SETTINGS_PATH);
            if (string.IsNullOrEmpty(directory))
            {
                Debug.LogError($"Directory not found for path: {DEV_SETTINGS_PATH}");
                return;
            }

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            System.IO.File.WriteAllText(DEV_SETTINGS_PATH, json);
        }
    }
#endif
}