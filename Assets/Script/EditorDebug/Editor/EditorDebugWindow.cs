#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace NgocDev.Core.EditorDebug
{
    public class EditorDebugWindow : EditorWindow
    {
        [MenuItem("NgocDev/Debug Window")]
        public static void ShowWindow() => GetWindow<EditorDebugWindow>("Debug Window").Show();
        
        private TypeCache.MethodCollection methods ;

        private void OnEnable()
        {
            methods = TypeCache.GetMethodsWithAttribute(typeof(DebugMethodAttribute));
        }
        private void CreateGUI()
        {
            foreach (var item in methods)
            {
                var debugButton = new Button();
                debugButton.text = item.Name;
                debugButton.clicked += () =>
                {
                    var parameters = item.GetParameters();
                    if (parameters.Length == 0)
                    {
                        item.Invoke(null, null);
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning($"Method {item.Name} has parameters. Cannot invoke.");
                    }
                };
                rootVisualElement.Add(debugButton);
            }

        }
    }
}
#endif