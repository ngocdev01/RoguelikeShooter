namespace NgocDev.Gameplay.MapGeneration.Editor
{
    using UnityEngine;
    using UnityEngine.UIElements;
    public class TilePreview : VisualElement
    {
        public static readonly string ussClassName = "tile-preview";
        public static readonly string thumbnailUssClassName = ussClassName + "__thumbnail";

    
        private VisualElement _thumbnail;
        private Texture2D _cachedIcon;
        private GameObject _cachedAsset;
        public TilePreview()
        {
            AddToClassList(ussClassName);
            _thumbnail = new VisualElement();
            _thumbnail.AddToClassList(thumbnailUssClassName);
            this.Add(_thumbnail);
        }

      

       
    }
}