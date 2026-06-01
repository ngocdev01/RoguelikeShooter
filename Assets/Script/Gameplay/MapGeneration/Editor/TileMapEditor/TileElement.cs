namespace NgocDev.Gameplay.MapGeneration.Editor
{
    using NgocDev.Gameplay.MapGeneration;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;


    public class TextureDebugWindow : EditorWindow
    {
        Texture2D texture;

        public static void Show(Texture2D tex)
        {
            var window = GetWindow<TextureDebugWindow>("Texture Debug");
            window.texture = tex;
            window.Repaint();
        }

        void OnGUI()
        {
            if (texture == null)
            {
                EditorGUILayout.LabelField("No texture assigned");
                return;
            }

            float aspect = (float)texture.width / texture.height;
            Rect rect = GUILayoutUtility.GetAspectRect(aspect);
            EditorGUI.DrawPreviewTexture(rect, texture);
        }
    }

    [System.Serializable]
    public class TileElement : BindableElement, INotifyValueChanged<TilePrefab>
    {
        public static readonly string ussClassName = "tile-element";
        public static readonly string thumbnailUssClassName = ussClassName + "__thumbnail";

        private TilePrefab _value;
        public TilePrefab value { get => _value; set => SetValueWithoutNotify(value); }

        private VisualElement _thumbnail;
        private GameObject _cachedAsset;

        public TileElement()
        {
            this.style.width = new Length(100, LengthUnit.Percent);
            this.style.height = new Length(100, LengthUnit.Percent);
            AddToClassList(ussClassName);
            _thumbnail = new VisualElement();
            _thumbnail.AddToClassList(thumbnailUssClassName);
            _thumbnail.style.width = new Length(100, LengthUnit.Percent);
            _thumbnail.style.height = new Length(100, LengthUnit.Percent);
            this.Add(_thumbnail);        
        }

     
        public void SetValueWithoutNotify(TilePrefab newValue)
        {
            _value = newValue;
            UpdateElement();
        }

        

        private void UpdateElement()
        {
            _cachedAsset = AssetDatabase.LoadAssetAtPath<GameObject>(_value.editorPath);
            var thumbnail = AssetPreview.GetAssetPreview(_cachedAsset);
            if(AssetPreview.IsLoadingAssetPreview(_cachedAsset.GetInstanceID()))
            {
                EditorApplication.update += UpdateThumbnail;
            }
            else
            {
                _thumbnail.style.backgroundImage = AssetPreview.GetAssetPreview(_cachedAsset);
            }
        }

   

        private void UpdateThumbnail()
        {
            if(!AssetPreview.IsLoadingAssetPreview(_cachedAsset.GetInstanceID()))
            {
                EditorApplication.update -= UpdateThumbnail;
                _thumbnail.style.backgroundImage = AssetPreview.GetAssetPreview(_cachedAsset);
                _thumbnail.style.backgroundColor = Color.purple;
              
            }        
        }
    }
}