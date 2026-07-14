using NgocDev.Editor;
using NgocDev.UI;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Core.Config
{
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            root.style.paddingTop = 4;
            root.style.paddingBottom = 4;
            root.styleSheets.Add(CustomElement.LoadMainStyleSheet());



            root.Add(BootstrapRegion());
            root.Add(EventRegion());






            return root;
        }


        private VisualElement BootstrapRegion()
        {
            var bootstrapRegion = new EditorFoldoutRegion("Bootstrap Scene");
            

            var bootstrapSceneProperty = serializedObject.FindProperty("bootstrapScene");
            var bootstrapSceneField = new PropertyField(bootstrapSceneProperty);
            
            bootstrapRegion.Add(bootstrapSceneField);
            
            return bootstrapRegion;
        }
        private VisualElement EventRegion()
        {
            var eventRegion = new EditorFoldoutRegion("Events");

            var eventChannelFolderField = new FolderPicker("Event Channel Folder");
            eventChannelFolderField.BindProperty(serializedObject.FindProperty("eventChannelFolder"));
            eventRegion.Add(eventChannelFolderField);
            return eventRegion;
        }


      
    }

}