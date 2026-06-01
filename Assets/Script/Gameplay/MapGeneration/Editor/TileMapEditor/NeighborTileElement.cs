using UnityEngine.UIElements;

namespace NgocDev.Gameplay.MapGeneration.Editor
{


    [UxmlElement]
    public partial class NeighborTileElement : VisualElement
    {
        private VisualElement _tileThumbnail;
        [UxmlAttribute("direction")]
        public NeighborDirection direction = NeighborDirection.North;

    }
       
}