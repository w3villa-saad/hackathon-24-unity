 #if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace W3Labs.Utils.Editor
{
    public class SceneNavigator : EditorWindow
    {
        private static string[] _activeScenes;
        private static string[] _inActiveScenes;
        private Vector2 _scrollPosition;
        private bool _isLoadingSceneAdditively;

        [MenuItem("W3Utils/Scenes", priority = 2)]
        public static void Init()
        {
            SceneNavigator window = EditorWindow.GetWindow<SceneNavigator>("Scene Navigator");
            window.Show();
        }

        [MenuItem("W3Utils/Play", priority = 1)]
        public static void Play()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // if (_activeScenes is null or { Length: 0 }) GetScenes();
                var sceneToLoad = EditorBuildSettings.scenes
                    .Where(scene => scene.enabled)
                    .Select(scene => scene.path)
                    .ToArray();
                
                if (sceneToLoad.Length>0) EditorSceneManager.OpenScene(sceneToLoad[0], OpenSceneMode.Single);
                EditorApplication.EnterPlaymode();
            }
        }

        private static string[] GetScenes()
        {
            return
            _activeScenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();
        }
        private void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));
            GUILayout.BeginVertical(GUI.skin.box);
            if (GUILayout.Button("PLAY")) { Play(); }
            _isLoadingSceneAdditively =  EditorGUILayout.Toggle("Load Scenes Additively", _isLoadingSceneAdditively, EditorStyles.toggle);
            if (SceneManager.sceneCount > 1)
            {
                if (GUILayout.Button("Unload All"))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(SceneManager.GetSceneAt(0).path, OpenSceneMode.Single);
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(20f);
            // string[] scenePaths = GetScenes();
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Active Scenes", EditorStyles.boldLabel);
            foreach (var scene in _activeScenes)
            {
                DrawSceneItem(scene, true);
            }
            GUILayout.EndVertical();
            GUILayout.Space(20f);
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Inactive Scenes", EditorStyles.boldLabel);
            foreach (var scene in _inActiveScenes)
            {
                DrawSceneItem(scene, false);
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void DrawSceneItem(string scene, bool isAssigned)
        {
            var sceneName = Path.GetFileNameWithoutExtension(scene);
            GUILayout.BeginHorizontal();
            GUILayout.Label(sceneName, EditorStyles.label);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Load"))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scene, _isLoadingSceneAdditively? OpenSceneMode.Additive: OpenSceneMode.Single);
                }
            }
            if (GUILayout.Button(isAssigned ? "Unassign" : "Assign"))
            {
                ToggleSceneAssignment(scene, isAssigned);
            }
            if (GUILayout.Button("Locate", EditorStyles.miniButtonRight))
            {
                var asset = AssetDatabase.LoadAssetAtPath<Object>(scene);
                if (asset != null)
                {
                    EditorApplication.ExecuteMenuItem("Window/General/Project");
                    Selection.activeObject = asset;
                    EditorGUIUtility.PingObject(asset);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void ToggleSceneAssignment(string scene, bool isAssigned)
        {
            EditorBuildSettingsScene newScene = new EditorBuildSettingsScene(scene, !isAssigned);
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            if (isAssigned)
                scenes.RemoveAll(s => s.path == scene);
            else
                scenes.Add(newScene);
            EditorBuildSettings.scenes = scenes.ToArray();
            LoadScenes();
        }
        
        private void OnFocus()
        {
            LoadScenes();
        }
        private void LoadScenes()
        {
            List<string> tempAssignedScenes = new List<string>();
            List<string> tempUnassignedScenes = new List<string>();

            string[] allScenes = AssetDatabase.FindAssets("t:scene");
            foreach (string sceneGUID in allScenes)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
                if (EditorBuildSettings.scenes.Any(s => s.path == scenePath))
                {
                    tempAssignedScenes.Add(scenePath);
                }
                else
                {
                    tempUnassignedScenes.Add(scenePath);
                }
            }

            // Assign the scenes after the iteration is complete
            _activeScenes = tempAssignedScenes.ToArray();
            _inActiveScenes = tempUnassignedScenes.ToArray();

            Repaint();
        }
    }
}
#endif