using NgocDev.Core.Config;
using UnityEditor;
using UnityEngine;

namespace NgocDev.Gameplay.MapGeneration.Editor
{
    [FilePath("Assets/Script/Gameplay/MapGeneration/Editor/Setting/MapEditorSetting.asset",FilePathAttribute.Location.ProjectFolder)]
    public class MapEditorSetting : ScriptableSingleton<MapEditorSetting>
    {
        [SerializeField]
        private Color _gridColor = Color.white;

        [SerializeField]
        [Range(0,1)]
        private float _gridOpacity = 0.5f;

        [SerializeField]
        private Material _previewMaterial;

        [SerializeField]
        private Color _previewColor = Color.white;

        public static float gridOpacity => instance._gridOpacity;
        public static Color gridColor => instance._gridColor;
        public static Material previewMaterial => instance._previewMaterial;
        public static Color previewColor => instance._previewColor;


        public static string filePath => GetFilePath();
        public static void Save()
        {
            instance.Save(false);
            var i = AssetDatabase.LoadAssetAtPath<MapEditorSetting>(filePath);
            Debug.Log(i);
        }

        private void OnValidate()
        {
            if(_previewMaterial != null)
            {
                _previewMaterial.color = _previewColor;
                EditorUtility.SetDirty(_previewMaterial);
            }
        }

    }

}
