namespace NgocDev.Core.Config.Editor
{
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

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

            var editor = Editor.CreateEditor(gameConfig);
            var inspector = editor.CreateInspectorGUI();
            inspector.Bind(editor.serializedObject);
            rootVisualElement.Add(inspector);
        }
    }
}