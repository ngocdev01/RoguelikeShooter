using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace NgocDev.Core.Editor.Config
{
    public class EditorConfigWindow : EditorWindow
    {
        private SerializedObject _serializedObject;

        [MenuItem("NgocDev/Editor Config")]
        public static void ShowWindow()
        {
            GetWindow<EditorConfigWindow>("Config");
        }

      

        private void CreateGUI()
        {
            rootVisualElement.Clear();

            _serializedObject = new SerializedObject(EditorConfig.instance);
            
            var gameConfig = _serializedObject.FindProperty("gameRuntimeConfig");
            var propertyField = new PropertyField(gameConfig);
            rootVisualElement.Add(propertyField);
            
            var useBoostrapScene = _serializedObject.FindProperty("useBoostrapSceneInEditor");
            var toggle = new SlideToggle("Use Boostrap Scene");
            toggle.BindProperty(useBoostrapScene);
            rootVisualElement.Add(toggle);
            rootVisualElement.Bind(_serializedObject);
        }

        
    }

    public static class EditorConfigUtility
    {
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        private static void PlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            }

            if (state == PlayModeStateChange.EnteredPlayMode && EditorConfig.instance.useBoostrapSceneInEditor)
            {
                _ = LoadBoostrapScene();
            }
        }

        private static async Awaitable LoadBoostrapScene()
        {
            EditorConfig config = EditorConfig.instance;
            string[] scenes = new string[SceneManager.sceneCount];
            for (int i =0;i< SceneManager.sceneCount;i++)
            {
                scenes[i] = SceneManager.GetSceneAt(i).path;
            }

            var path = AssetDatabase.GetAssetPath(config.gameRuntimeConfig.bootstrapScene.editorAsset);
            await LoadSceneAsync(path, LoadSceneMode.Single);
            foreach (var scene in scenes)
            {
                await LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }
        private static async Awaitable LoadSceneAsync(string scenePath,LoadSceneMode loadSceneMode)
        {
            await EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath, new  LoadSceneParameters(loadSceneMode));
        }
    }
}