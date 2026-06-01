using NgocDev.Addressable;
using NgocDev.Addressable.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NgocDev.Core.Addressable.Editor
{
    [CustomPropertyDrawer(typeof(AddressableAsset))]
    public class AddressableAssetDrawer : PropertyDrawer
    {
        private UnityEditor.Search.ObjectField objectField;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var type = property.serializedObject.targetObject.GetType();

            var prop = type.GetProperty(property.name);
            if (prop != null)
            {
                prop.GetCustomAttributes(typeof(AssetInfoAttribute), false);
            }
            var objectField = new AddressableObjectField(property.displayName);
            objectField.BindProperty(property.FindPropertyRelative("address"));
            root.Add(objectField);
            return root;

        }

    }
}