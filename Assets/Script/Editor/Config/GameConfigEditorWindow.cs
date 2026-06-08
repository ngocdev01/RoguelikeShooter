namespace NgocDev.Core.Config.Editor
{
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using UnityEditor.Compilation;

    class GameConfigEditorWindow : EditorWindow
    {
        private GameConfig gameConfig;

        [MenuItem("NgocDev/Game Config Editor")]
        public static void ShowWindow()
        {
            GameConfigEditorWindow window = GetWindow<GameConfigEditorWindow>();
            window.titleContent = new GUIContent("Game Config",
            EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow").image);
           
            window.Show();

        }

        private void OnEnable()
        {
            LoadGameConfig();
        }

        private void LoadGameConfig()
        {
            gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/Config/GameConfig.asset");
        }

        private void VerifyGameSettingInstance()
        {
            var types = TypeCache.GetTypesDerivedFrom(typeof(ScriptableSetting<>));
            foreach (var type in types)
            {
                var gameSettingAttribute = type.GetCustomAttribute<ScriptableSettingAttribute>(false);
                if (gameSettingAttribute != null)
                {
                    var instance = AssetDatabase.LoadAssetAtPath(gameSettingAttribute.editorFilePath,type);           
                    if (instance == null)
                    {
                        instance = CreateInstance(type);
                        AssetDatabase.CreateAsset(instance, gameSettingAttribute.editorFilePath);
                        AssetDatabase.SaveAssets();
                    }
                    var addressablSettings = AddressableAssetSettingsDefaultObject.Settings;      
                    var entry = addressablSettings.FindAssetEntry(AssetDatabase.AssetPathToGUID(gameSettingAttribute.editorFilePath));
                    if (entry == null)
                    {
                        var group = addressablSettings.FindGroup("Game Config");
                        entry = addressablSettings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(gameSettingAttribute.editorFilePath),group);
                    }
                    if (entry.address != gameSettingAttribute.runtimeKey)
                    {
                        entry.SetAddress(gameSettingAttribute.runtimeKey);
                    }
                }
            }
        }

        public void CreateGUI()
        {
    
            rootVisualElement.Clear();
            
            if (gameConfig == null)
            {
                var label = new Label("Game Config not loaded. Check console for errors.");
                rootVisualElement.Add(label);
                return;
            }
            var title = new Label("Game Config");

      
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 18;
            title.style.marginBottom = 10; 

            rootVisualElement.Add(title);


            var verifyButton = new Button(VerifyGameSettingInstance) { text = "Verify Game Settings" };
            rootVisualElement.Add(verifyButton);

            var editor = Editor.CreateEditor(gameConfig);
            var inspector = editor.CreateInspectorGUI();
            inspector.Bind(editor.serializedObject);
            rootVisualElement.Add(inspector);
        }
    }
}