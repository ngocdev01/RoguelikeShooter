using UnityEditor;
using UnityEngine;

namespace NgocDev.Editor
{
    public class EditorUtility
    {
        public const string EDITOR_DEFAULT_PATH = "Assets/Script/Editor/";
        public const string EDITOR_ASSET_PATH = "Assets/Script/Editor/EditorAsset/";
        public const string CORE_ASSET_PATH = "Assets/Asset";
        public static Object LoadAsset<T>(string assetPath) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(CORE_ASSET_PATH + assetPath);
        }
    }
}