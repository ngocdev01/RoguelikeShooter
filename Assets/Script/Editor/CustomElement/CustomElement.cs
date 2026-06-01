using UnityEditor;
using UnityEngine.UIElements;

namespace NgocDev.Editor
{
    public class CustomElement
    {
        public static StyleSheet LoadMainStyleSheet() =>
            AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Script/Editor/CustomElement/CustomElements.uss");
        public static StyleSheet LoadSearchStyleSheet() =>
            AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Script/Editor/CustomElement/Search/SearchElement.uss");
    }
}