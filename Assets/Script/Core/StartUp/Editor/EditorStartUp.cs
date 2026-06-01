#if UNITY_EDITOR
namespace NgocDev.Core.StartUp
{
    using NgocDev.Core.Config;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class EditorStartUp
    {
        private static string boostrapScenePath => AssetDatabase.GetAssetPath(GameConfig.instance.bootstrapScene.editorAsset);

        private static bool isDefaultSceneLoaded => IsDefaultSceneLoaded();

        [InitializeOnLoadMethod]
        static void OnEnterPlaymodeInEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        [InitializeOnLoadMethod]
        static void PersistentScene()
        {
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneOpened;
        }

        private static bool IsDefaultSceneLoaded()
        {
            var sceneCount = EditorSceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                if (scene.path == boostrapScenePath)
                {
                    return true;
                }
            }
            return false;
        }

        private static void OnSceneOpened(Scene arg0, Scene arg1)
        {           
            OpenDefaultScene();
            EditorSceneManager.SetActiveScene(arg1);
        }


        private static void OpenDefaultScene()
        {
            if (isDefaultSceneLoaded) return;
            var scene = EditorSceneManager.OpenScene(boostrapScenePath, OpenSceneMode.Additive);
               
            var first = EditorSceneManager.GetSceneAt(0);
            if(first == scene)
            {
                return;
            }
            EditorSceneManager.MoveSceneBefore(scene, first);
            foreach (var go in scene.GetRootGameObjects())
            {
                go.hideFlags = HideFlags.NotEditable;
            }
        }

        public static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
            else if (change == PlayModeStateChange.EnteredPlayMode)
            {
                _ = OnPlayModeStateChangedAsync();
            }
        }



        private static async Awaitable OnPlayModeStateChangedAsync()
        {

            if (!GameConfig.instance.useBoostrapScene)
            {
                return;
            }

            var currentScene = SceneManager.GetActiveScene().path;
            await LoadSceneAsync(boostrapScenePath, LoadSceneMode.Single);

            // Wait for end of frame to ensure all Awake/OnEnable callbacks 
            // in bootstrap scene are executed before loading additional scenes
            await Awaitable.EndOfFrameAsync();

            await LoadSceneAsync(currentScene, LoadSceneMode.Additive);


        }

        private static async Awaitable LoadSceneAsync(string scenePath, LoadSceneMode loadSceneMode)
        {
            await EditorSceneManager.LoadSceneAsyncInPlayMode(
                          scenePath,
                          new LoadSceneParameters(loadSceneMode));
        }
    }
}
#endif