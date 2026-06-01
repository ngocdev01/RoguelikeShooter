namespace NgocDev.UI.Editor
{
    using UnityEditor;
    using NgocDev.UI;
    using UnityEngine.UIElements;
    using UnityEditor.UIElements;

    [CustomPropertyDrawer(typeof(FolderFieldAttribute))]
    public class FolderFieldDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var folderField = new FolderPicker(property.displayName);
            folderField.BindProperty(property);
            return folderField;
        }
    }

}